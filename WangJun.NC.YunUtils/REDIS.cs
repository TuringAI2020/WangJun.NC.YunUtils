using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WangJun.NC.YunUtils
{
    public class REDIS
    {
        private static ConnectionMultiplexer redis = null;
        private IDatabase db = null;
        public static REDIS GetInst(int db = 0)
        {
            if (null == redis)
            {
                redis = ConnectionMultiplexer.Connect("127.0.0.1:7379");
            }
            var inst = new REDIS();
            inst.db = redis.GetDatabase(0);
            return inst;
        }

        public static REDIS Current
        {
            get
            {
                var inst = REDIS.GetInst();
                return inst;
            }
        }

        public RES CacheSet(string key, object val, TimeSpan? expiry=null) 
        {
            try
            {
                var res = false;
                if (val is string)
                {
                    res = this.db.StringSet(key, val as string, expiry);
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
                 var val =  this.db.StringGet(key);
                if (!val.IsNullOrEmpty)
                {
                    if (typeof(T) == typeof(string))
                    {
                        return RES.OK(val);
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

        public RES Enqueue(string queueName, object param)
        {
            try
            {
                if (param is string && !string.IsNullOrWhiteSpace(param as string))
                {
                    this.db.ListRightPush(queueName, param as string);
                }
                else if (null != param && param is object)
                {
                    this.db.ListRightPush(queueName, JSON.ToJson(param));
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
                    else
                    {
                        var json = JSON.ToObject<T>(val);
                        return RES.OK(json);
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
    }
}
