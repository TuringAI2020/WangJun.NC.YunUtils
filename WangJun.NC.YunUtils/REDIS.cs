using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WangJun.NC.YunUtils
{
    public class REDIS
    {
        private static ConnectionMultiplexer redis = null;
        private IDatabase db = null;
        private IServer server = null;
        private static REDIS inst = null;
        public static REDIS GetInst(int db = 0, string connection = "127.0.0.1:6379")
        {
            if (null == redis)
            {
                redis = ConnectionMultiplexer.Connect(connection);
            }
            if (null == REDIS.inst)
            {
                REDIS.inst = new REDIS();
                REDIS.inst.db = redis.GetDatabase(0);
                REDIS.inst.server = redis.GetServer(connection);
            }
            return REDIS.inst;
        }

        public static REDIS Current
        {
            get
            {
                var inst = REDIS.GetInst();
                return inst;
            }
        }

        public RES CacheSet(string key, object val, TimeSpan? expiry = null)
        {
            try
            {
                var res = false;
                if (val is string)
                {
                    res = this.db.StringSet(key, val as string, expiry);
                }
                else if (val is int)
                {
                    res = this.db.StringSet(key, (int)val, expiry);
                }
                else if (val is long)
                {
                    res = this.db.StringSet(key, (long)val, expiry);
                }
                else if (val is object)
                {
                    res = this.db.StringSet(key, JSON.ToJson(val), expiry);
                }

                return RES.OK(res);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES CacheGet<T>(string key)
        {
            try
            {
                var val = this.db.StringGet(key);
                if (!val.IsNullOrEmpty)
                {
                    if (typeof(T) == typeof(string))
                    {
                        return RES.OK(val.ToString());
                    }
                    else
                    {
                        var jobj = JSON.ToObject<T>(val);
                        return RES.OK(jobj);
                    }
                }
                return RES.FAIL("未找到有效数据");
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES QueryKeys(string match,int db=0)
        {
            try
            {
                var keys = server.Keys(db,match,int.MaxValue).Select(p=>p.ToString()).ToList();
                return RES.OK(keys)  ;
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES KeyExists(string key)
        {
            try
            {
                var val = this.db.KeyExists(key);
                return (val) ? RES.OK(val) : RES.FAIL(val);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }


        public RES KeyExists(string[] keys)
        {
            try
            {
                var rKeys = new List<RedisKey>();
                keys.ToList().ForEach(p =>
                {
                    rKeys.Add(p);
                });
                var val = this.db.KeyExists(rKeys.ToArray());
                return (0 < val) ? RES.OK(val) : RES.FAIL(val);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES KeyRemove(string key)
        {
            try
            {
                var val = this.db.KeyDelete(key);
                return (val) ? RES.OK(val) : RES.FAIL(val);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES KeyRemove(string[] keys)
        {
            try
            {
                var rKeys = new List<RedisKey>();
                keys.ToList().ForEach(p =>
                {
                    rKeys.Add(p);
                });
                var val = this.db.KeyDelete(rKeys.ToArray());
                return (0 < val) ? RES.OK(val) : RES.FAIL(val);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        #region 队列 
        public RES Enqueue(string queueName, object val)
        {
            try
            {
                if (val is string)
                {
                    this.db.ListRightPush(queueName, val as string);
                }
                else if (val.GetType() == typeof(DateTime))
                {
                    this.db.ListRightPush(queueName, val.ToString());
                }
                else if (val.GetType() == typeof(int))
                {
                    this.db.ListRightPush(queueName, (int)val);
                }
                else if (val.GetType() == typeof(long))
                {
                    this.db.ListRightPush(queueName, (long)val);
                }
                else if (val.GetType() == typeof(decimal))
                {
                    this.db.ListRightPush(queueName, (double)val);
                }
                else if (val.GetType() == typeof(float))
                {
                    this.db.ListRightPush(queueName, (double)val);
                }
                else if (val.GetType() == typeof(double))
                {
                    this.db.ListRightPush(queueName, (double)val);
                }
                else if (val.GetType() == typeof(Guid))
                {
                    this.db.ListRightPush(queueName, val.ToString());
                }
                else if (val.GetType().IsClass)
                {
                    this.db.ListRightPush(queueName, JSON.ToJson(val));
                }
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES Dequeue<T>(string queueName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(queueName))
                {
                    var val = this.db.ListLeftPop(queueName);
                    if (typeof(T) == typeof(string))
                    {
                        return RES.OK(val.ToString());
                    }
                    else if (typeof(T).IsClass)
                    {
                        var json = JSON.ToObject<T>(val);
                        return RES.OK(json);
                    }
                    else
                    {
                        return RES.OK(val.Box());
                    }
                }
                return RES.FAIL();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES GetQueueLength(string queueName)
        {
            var length = this.db.ListLength(queueName);
            return RES.OK(length);
        }

        #endregion

        #region 字典
        public RES DictAdd(string dictName, string key, object val)
        {
            try
            {
                if (val is string)
                {
                    this.db.HashSet(dictName, key, val as string);
                }
                else if (val.GetType() == typeof(DateTime))
                {
                    this.db.HashSet(dictName, key, val.ToString());
                }
                else if (val.GetType() == typeof(int))
                {
                    this.db.HashSet(dictName, key, (int)val);
                }
                else if (val.GetType() == typeof(long))
                {
                    this.db.HashSet(dictName, key, (long)val);
                }
                else if (val.GetType() == typeof(decimal))
                {
                    this.db.HashSet(dictName, key, Convert.ToDouble(val));
                }
                else if (val.GetType() == typeof(float))
                {
                    this.db.HashSet(dictName, key, (float)val);
                }
                else if (val.GetType() == typeof(double))
                {
                    this.db.HashSet(dictName, key, (double)val);
                }
                else if (val.GetType() == typeof(Guid))
                {
                    this.db.HashSet(dictName, key, val.ToString());
                }
                else if (val.GetType().IsClass)
                {
                    this.db.HashSet(dictName, key, JSON.ToJson(val));
                }

                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES DictGet(string dictName, string key)
        {
            try
            {
                var val = this.db.HashGet(dictName, key);
                if (val.IsNullOrEmpty)
                {
                    return RES.FAIL(null);
                }
                return RES.OK(val.ToString());
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES DictRemove(string dictName, string key)
        {
            try
            {
                var val = this.db.HashDelete(dictName, key);
                return (val) ? RES.OK(val) : RES.FAIL(val);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES DictGetList(string dictName, List<string> keys)
        {
            try
            {
                var values = new List<RedisValue>();
                keys.ForEach(p =>
                {
                    values.Add(p);
                });
                var val = this.db.HashGet(dictName, values.ToArray()).ToList();
                var resArr = new List<string>();
                val.ForEach(p =>
                {
                    resArr.Add(p.ToString());
                });
                return RES.OK(resArr);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES DictScan(string dictName, string search, int count)
        {
            try
            {
                var list = new List<string>();
                var res = this.db.HashScan(dictName, search, count).ToList();
                res.ForEach(p =>
                {
                    list.Add(p.Value.ToString());
                });
                return RES.OK(list);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
         

        public RES DictTraverse(string dictName, string search, Func<string, string, string, int, int, bool> callback)
        {
            try
            { 
                var resQuery = this.db.HashScan(dictName, search, int.MaxValue);
                var count =(int)resQuery.Count();

                var index = 0;
                foreach (var item in resQuery)
                {
                    if (null != callback)
                    {
                        var _continue = callback(dictName, item.Name, item.Value, count, index++);
                        if (!_continue)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                } 
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES SetTraverse(string dictName, string search, Func<string, string, int, int, bool> callback)
        {
            try
            {
                var count = (int)(this.db.SetLength(dictName) % int.MaxValue);
                var resQuery = this.db.SetScan(dictName, search, count);
                var index = 0;
                foreach (var item in resQuery)
                {
                    if (null != callback)
                    {
                        var _continue = callback(dictName, item, count, index++);
                        if (!_continue)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
        #endregion

        #region 有序集合

        public RES SortedSetAdd(string setName, object val, double sort)
        {
            if (val is string)
            {
                this.db.SortedSetAdd(setName, val as string, sort);
            }
            else if (val.GetType() == typeof(DateTime))
            {
                this.db.SortedSetAdd(setName, val.ToString(), sort);
            }
            else if (val.GetType() == typeof(int))
            {
                this.db.SortedSetAdd(setName, (int)val, sort);
            }
            else if (val.GetType() == typeof(long))
            {
                this.db.SortedSetAdd(setName, (long)val, sort);
            }
            else if (val.GetType() == typeof(decimal))
            {
                this.db.SortedSetAdd(setName, (double)val, sort);
            }
            else if (val.GetType() == typeof(float))
            {
                this.db.SortedSetAdd(setName, (double)val, sort);
            }
            else if (val.GetType() == typeof(double))
            {
                this.db.SortedSetAdd(setName, (double)val, sort);
            }
            else if (val.GetType() == typeof(Guid))
            {
                this.db.SortedSetAdd(setName, val.ToString(), sort);
            }
            else if (val.GetType().IsClass)
            {
                this.db.SortedSetAdd(setName, JSON.ToJson(val), sort);
            }
            return RES.OK();
        }

        public RES SortedSetScan(string setName, string search, int pageSize)
        {
            try
            {
                var list = new List<string>();
                var res = this.db.SortedSetScan(setName, search, pageSize).ToList();
                res.ForEach(p =>
                {
                    list.Add(p.Element.ToString());
                });
                return RES.OK(list);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES SortedSetRemoveByScore(string setName,int start=int.MinValue,int stop=int.MaxValue)
        {
            try
            {
                var list = new List<string>();
                var res = this.db.SortedSetRangeByScore(setName, start,stop).ToList();
                res.ForEach(p =>
                {
                    list.Add(p.ToString());
                });
                return RES.OK(list);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES SortedSetGetOnceByScore(string setName, int queryScore=0,int tempScore=0)
        {
            try
            {
                var list = new List<string>();
                var res = this.db.SortedSetRangeByScore(setName, queryScore, queryScore, Exclude.None, Order.Descending, 0, 1).ToList();
                if (0 < res.Count) {
                    var data = res.First().ToString();
                    var res2 = this.db.SortedSetAdd(setName, data, tempScore);
                    return RES.OK(data);
                }
                return RES.FAIL(res.Count);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES SortedSetTraverse(string dictName, string search, Func<string, string, double, int, int, bool> callback)
        {
            try
            {
                var resQuery = this.db.SortedSetScan(dictName, search, int.MaxValue);
                var count = (int)resQuery.Count();

                var index = 0;
                foreach (var item in resQuery)
                {
                    if (null != callback)
                    {
                        var _continue = callback(dictName, item.Element.ToString(), item.Score, count, index++);
                        if (!_continue)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES SortedSetQuery(string setName, double start, double stop, bool isDesc, int pageIndex, int pageSize)
        {
            try
            {
                var list = new List<string>();
                var order = isDesc ? Order.Descending : Order.Ascending;
                var res = this.db.SortedSetRangeByScore(setName, start, stop, Exclude.None, order, (pageIndex * pageSize), pageSize).ToList();
                res.ForEach(p =>
                {
                    list.Add(p.ToString());
                });
                return RES.OK(list);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES SortedSetRemove(string setName, object val)
        {
            if (val is string)
            {
                this.db.SortedSetRemove(setName, val as string);
            }
            else if (val.GetType() == typeof(DateTime))
            {
                this.db.SortedSetRemove(setName, val.ToString());
            }
            else if (val.GetType() == typeof(int))
            {
                this.db.SortedSetRemove(setName, (int)val);
            }
            else if (val.GetType() == typeof(long))
            {
                this.db.SortedSetRemove(setName, (long)val);
            }
            else if (val.GetType() == typeof(decimal))
            {
                this.db.SortedSetRemove(setName, (double)val);
            }
            else if (val.GetType() == typeof(float))
            {
                this.db.SortedSetRemove(setName, (double)val);
            }
            else if (val.GetType() == typeof(double))
            {
                this.db.SortedSetRemove(setName, (double)val);
            }
            else if (val.GetType() == typeof(Guid))
            {
                this.db.SortedSetRemove(setName, val.ToString());
            }
            else if (val.GetType().IsClass)
            {
                this.db.SortedSetRemove(setName, JSON.ToJson(val));
            }
            return RES.OK();
        }
        #endregion

        #region 列表
        public RES GetListLastItems(string listName,int start=-20,int end=-1)
        {
            try
            {
                var list = new List<string>();
                var items = this.db.ListRange(listName, start, end);
                items.ToList().ForEach(p =>
                {
                    list.Add(p);
                }); 
                return (0 < list.Count) ? RES.OK(list) : RES.FAIL();
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
        public RES RemoveListItem(string listName, string item, int count=0)
        {
            try
            {
                var list = new List<string>();
                var resCount = this.db.ListRemove(listName,item,count); 
                return (0 < resCount) ? RES.OK(resCount) : RES.FAIL($"{listName}\t{resCount}\t{item}");
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
        #endregion




        public RES Execute(string serverId, string command, object[] paramArr)
        {
            try
            {
                var res = (RedisValue[])REDIS.redis.GetDatabase().Execute(command, paramArr);
                return RES.OK(res.ToStringArray());
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
    }
}
