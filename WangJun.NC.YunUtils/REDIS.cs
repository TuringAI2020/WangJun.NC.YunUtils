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
        private static REDIS inst = null;
        public static REDIS GetInst(int db = 0, string connection = "127.0.0.1:6379")
        {
            if (null == redis)
            {
                redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            }
            if (null == REDIS.inst)
            {
                REDIS.inst = new REDIS();
                REDIS.inst.db = redis.GetDatabase(0);
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
                var rKeys = new RedisKey[keys.Length];
                for (int k = 0; k < keys.Length; k++)
                {
                    rKeys[k] = keys[k];
                }
                var val = this.db.KeyExists(rKeys);
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

        public RES DictGetList(string dictName, List<string> keys)
        {
            try
            {
                var values = new List<RedisValue>();
                keys.ForEach(p=> {
                    values.Add(p);
                });
                var val = this.db.HashGet(dictName, values.ToArray()).ToList();
                var resArr = new List<string>();
                val.ForEach(p=> {
                    resArr.Add(p.ToString());
                });
                return RES.OK(resArr);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES DictScan(string dictName, string search,int count)
        {
            try
            {
                var list = new List<string>();
                var res = this.db.HashScan(dictName, search,count).ToList();
                res.ForEach(p=> {
                    list.Add(p.Value.ToString());
                });
                return RES.OK(list);
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

        public RES SortedSetScan(string setName , string search,int pageSize) {
            try
            {
                var list = new List<string>();
                var res = this.db.SortedSetScan(setName, search, pageSize).ToList();
                res.ForEach(p=> {
                    list.Add(p.Element.ToString());
                });
                return RES.OK(list);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        public RES SortedSetQuery(string setName, double start,double stop,bool isDesc,int pageIndex, int pageSize)
        {
            try
            {
                var list = new List<string>();
                var order = isDesc ? Order.Descending : Order.Ascending;
                var res = this.db.SortedSetRangeByScore(setName, start, stop, Exclude.None, order, (pageIndex * pageSize), pageSize).ToList();
                res.ForEach(p=> {
                    list.Add(p.ToString());
                });
                return RES.OK(list);
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
