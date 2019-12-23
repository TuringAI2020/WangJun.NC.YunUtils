using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public class RedisDB
    {
        public Task task = null;
        public RES Save<T>(T t) where T : Item, new()
        {
            try
            {
                ///记录要保存的数据 
                this.SaveFlat(t);
                this.SaveToList(t);
                this.BuildSearchIndex(t);
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        protected RES SyncToDB<T>(T t) where T : Item, new()
        {
            try
            {
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
         

        protected RES SaveModifyNotice<T>(T  t) where T : Item, new()
        {
            try
            {
                if (null == t || t.ItemID == Guid.Empty)
                {
                    return RES.FAIL();
                }

                var key = $"MODIFY:{t.GetType().FullName.Replace(".", ":")}";
                REDIS.Current.Enqueue(key, new ChangeNotice{ ClassFullName= t.GetType().FullName, ItemID=t.ItemID  , CreateTime=DateTime.Now.Ticks} );

                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
         

        protected RES FinishCheckPoint<T>(T t) where T : Item, new()
        {
            try
            {
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        protected RES SaveFlat<T>(T t) where T : Item, new()
        {
            try
            {
                const string prefix= "FLAT";
                if (null != t)
                {
                    ///展开存储
                    var properties = t.GetType().GetProperties().ToList();
                    var classFullName = $"{prefix}:{t.GetType().FullName.Replace(".", ":")}"; 

                    var oldVal =REDIS.Current.DictGet($"{classFullName}", t.ItemID.ToString());

                    ///检测是否存在
                    var keys = properties.Where(p => p.CanWrite && p.CanWrite).Select(p => $"{classFullName}:{t.ItemID}:{p.Name}").ToArray();
                    var flatRes = REDIS.Current.KeyExists(keys);

                    if (null != oldVal && 0 == ((long)flatRes.DATA)) ///平铺
                    {
                        properties.ForEach(p =>
                        {
                            if (p.CanWrite && p.CanRead)
                            {
                                var key = $"{classFullName}:{t.ItemID}:{p.Name}";

                                REDIS.Current.CacheSet(key, p.GetValue(t), new TimeSpan(0, 5, 0));
                            }
                        });
                    }

                    if (null == oldVal) ///全新对象
                    {
                        properties.ForEach(p =>
                        {
                            if (p.CanWrite && p.CanRead)
                            {
                                var key = $"{classFullName}:{t.ItemID}:{p.Name}";

                                REDIS.Current.CacheSet(key, p.GetValue(t), new TimeSpan(0, 5, 0));
                            }
                        });
                    }
                    else if (null != oldVal && keys.Length == ((long)flatRes.DATA)) ///更新对象
                    {
                        properties.ForEach(p =>
                        {
                            var attrs = p.GetCustomAttributes(typeof(ReadonlyAttribute), true);
                            var key = $"{classFullName}:{t.ItemID}:{p.Name}";
                            if (0 == attrs.Length)
                            {
                                REDIS.Current.CacheSet(key, p.GetValue(t), new TimeSpan(0, 5, 0));
                            }
                        });
                    }
                }

                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        protected RES SaveToList<T>(T t) where T : Item, new()
        {
            try
            {
                const string prefix = "LIST";
                if (null != t)
                { 
                    var classFullName = $"{prefix}:{t.GetType().FullName.Replace(".", ":")}"; 
                    REDIS.Current.DictAdd($"{classFullName}", t.ItemID.ToString(), t);
                    this.SaveModifyNotice(t);
                }
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        protected RES BuildSearchIndex<T>(T t) where T : Item, new()
        {
            try
            {
                const string prefix = "INDEX";
                if (null != t)
                {
                    ///展开存储
                    var properties = t.GetType().GetProperties().ToList();
                    var classFullName =$"{prefix}:{t.GetType().FullName.Replace(".", ":")}" ;
                    properties.ForEach(p =>
                    {
                        if (p.CanWrite && p.CanRead)
                        {
                            var val = p.GetValue(t);
                            var key = $"{classFullName}:{p.Name}";
                            if (p.PropertyType == typeof(DateTime))
                            {
                                var tick = ((DateTime)val).Ticks;
                                REDIS.Current.SortedSetAdd(key, t.ItemID, tick);
                            }
                            else if (p.PropertyType == typeof(int))
                            {
                                REDIS.Current.SortedSetAdd(key, t.ItemID, Convert.ToDouble(val));
                            }
                            else if (p.PropertyType == typeof(long))
                            {
                                REDIS.Current.SortedSetAdd(key, t.ItemID, Convert.ToDouble(val));
                            }
                            else if (p.PropertyType == typeof(decimal))
                            {
                                REDIS.Current.SortedSetAdd(key, t.ItemID, Convert.ToDouble(val));
                            }
                            else if (p.PropertyType == typeof(float))
                            {
                                REDIS.Current.SortedSetAdd(key, t.ItemID, Convert.ToDouble(val));
                            }
                            else if (p.PropertyType == typeof(double))
                            {
                                REDIS.Current.SortedSetAdd(key, t.ItemID, Convert.ToDouble(val));
                            }
                            else if (p.PropertyType == typeof(string))
                            {
                                var attr = p.GetCustomAttribute<KeywordAttribute>();
                                if (null != attr&& null != val)
                                { 
                                    var oldVal = REDIS.Current.DictGet(key, val.ToString());
                                    if (!oldVal.SUCCESS || null == oldVal.DATA)
                                    {
                                        REDIS.Current.DictAdd(key, val.ToString(), JSON.ToJson(new List<string> { t.ItemID.ToString() }));
                                    }
                                    else if (oldVal.SUCCESS && null != oldVal.DATA)
                                    {
                                        var oldJson = JSON.ToObject<List<string>>(oldVal.DATA.ToString());
                                        oldJson.Add(t.ItemID.ToString());
                                        REDIS.Current.DictAdd(key, val.ToString(), JSON.ToJson( oldJson));
                                    }
                                    
                                }
                            }
                        }
                    });
                } 
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES QueryList<T>(Dictionary<string,object> paramDict)
        {
            try
            {
                var classFullName = $"{typeof(T).FullName.Replace(".", ":")}";
                var list = new List<string>();
                if (null == paramDict||0==paramDict.Count)
                {
                    return RES.FAIL();
                }
                if (paramDict.ContainsKey("Title") && paramDict.ContainsKey("Title"))
                {
                    var keyword = paramDict["Title"].ToString();
                    var res = REDIS.Current.DictScan($"INDEX:{classFullName}:Title", $"*{keyword}*", 10);
                    if (res.SUCCESS)
                    {
                        var resList = res.DATA as List<string>;
                        var idList = new List<string>();
                        resList.ForEach(p=> {
                            var itemIds =JSON.ToObject<List<string>>(p);
                            itemIds.ForEach(w=> {
                                idList.Add(w);
                            });
                        });

                        list.AddRange(idList);
                    }
                }

                if (paramDict.ContainsKey("CreateTimeStart") && paramDict.ContainsKey("CreateTimeEnd"))
                {
                    var createTimeStart = Convert.ToDateTime(paramDict["CreateTimeStart"].ToString());
                    var createTimeEnd = Convert.ToDateTime(paramDict["CreateTimeEnd"].ToString());
                    var res = REDIS.Current.SortedSetQuery($"INDEX:{classFullName}:CreateTime", createTimeStart.Ticks, createTimeEnd.Ticks, true, 0, 10);
                    if (res.SUCCESS)
                    {
                        var idList = res.DATA as List<string>;
                        list.Intersect(idList);
                    }
                }
                if (paramDict.ContainsKey("UpdateTimeStart") && paramDict.ContainsKey("UpdateTimeEnd"))
                {
                    var updateTimeStart = Convert.ToDateTime(paramDict["UpdateTimeStart"].ToString());
                    var updateTimeEnd = Convert.ToDateTime(paramDict["UpdateTimeEnd"].ToString());
                    var res = REDIS.Current.SortedSetQuery($"INDEX:{classFullName}:UpdateTime", updateTimeStart.Ticks, updateTimeEnd.Ticks, true, 0, 10);
                    if (res.SUCCESS)
                    {
                        var idList = res.DATA as List<string>;
                        list = list.Intersect(idList).ToList();
                    }
                }  
                var items = REDIS.Current.DictGetList($"LIST:{classFullName}", list);
                return RES.OK(items);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
    }

    public class Item
    {
        [Readonly]
        public Guid ItemID { get; set; }

        [Readonly]
        public string ClassFullName { get; set; }

        public bool IsEmpty { get; }
    }

    public class ChangeNotice
    {
        [Readonly]
        public Guid ItemID { get; set; }

        [Readonly]
        public string ClassFullName { get; set; }

        [Readonly]
        public long CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
    }

    public class ReadonlyAttribute : Attribute { }

    public class NotMappedAttribute : Attribute { }

    public class KeywordAttribute : Attribute { }
}
