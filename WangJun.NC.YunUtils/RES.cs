using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WangJun.NC.YunUtils.ENUM;

namespace WangJun.NC.YunUtils
{
    public class RES
    {
        public bool SUCCESS { get; set; }

        public int CODE { get; set; }

        public string MESSAGE { get; set; }

        public object DATA { get; set; }

        public int DataType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int PageCount
        {
            get
            {
                if (0 < PageSize)
                {
                    return (0 == TotalCount % PageSize) ? TotalCount / PageSize : 1 + (TotalCount / PageSize);
                }
                return 0;
            }
        }

        public static RES New
        {
            get
            {
                var inst = new RES();
                return inst;
            }
        }

        /// <summary>
        /// 成功结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public RES SetAsOK(object data = null, string msg = "成功")
        {
            this.SUCCESS = true;
            if (null != data)
            {
                this.DATA = data;
            }
            this.MESSAGE = msg;
            return this;
        }

        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public RES SetAsFail(object data = null, string msg = "失败")
        {
            this.SUCCESS = false;
            this.DATA = data;
            this.MESSAGE = msg;
            return this;
        }

        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static RES FAIL(object data = null, string msg = "失败")
        {
            var res = new RES();
            res.SUCCESS = false;
            res.DATA = data;
            res.MESSAGE = msg;
            return res;
        }

        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static RES FAIL(Exception ex)
        {
            var res = new RES();
            res.SUCCESS = false;
            if (ex is Exception)
            {
                res.DATA = $"{ex.Message}\t{((null == ex.InnerException) ? string.Empty : ex.InnerException.Message)}";
            }
            else
            {
                res.DATA = null;
            }
            res.MESSAGE = "发生异常";
            return res;
        }

        public static RES FAIL(Exception ex, 数据类型 dataType)
        {
            var res = new RES();
            res.SUCCESS = false;
            res.DATA = $"{ex.Message}\t{((null == ex.InnerException) ? string.Empty : ex.InnerException.Message)}";
            var msg = $"{ex.Message}\t{((null == ex.InnerException) ? string.Empty : ex.InnerException.Message)}";
            if (dataType == 数据类型.ByteArray)
            {
                res.DATA = Encoding.UTF8.GetBytes(msg);
            }
            else
            {

            }
            res.MESSAGE = "发生异常";
            res.DataType = (int)dataType;

            return res;
        }

        /// <summary>
        /// 成功结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static RES OK(object data = null, string msg = "成功")
        {
            var res = new RES();
            res.SUCCESS = true;
            res.DATA = data;
            res.MESSAGE = msg;
            return res;
        }

        public static RES OK(object data , 数据类型 dataType, string msg = "成功")
        {
            var res = new RES();
            res.SUCCESS = true;
            if (null != data && dataType == 数据类型.ByteArray)
            {
                var dataStr = string.Empty;
                if (typeof(string) == data.GetType())
                {
                    res.DATA = Encoding.UTF8.GetBytes(data.ToString());
                }
                else
                {
                    res.DATA = Encoding.UTF8.GetBytes(JSON.ToJson(data));
                }
            }
            else if(null == data && dataType == 数据类型.ByteArray)
            {
                res.DATA = new byte[1];
            } 
            res.MESSAGE = msg;
            res.DataType = (int)dataType;
            return res;
        }

        /// <summary>
        /// 转换为JSON
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JSON.ToJson(this);
        }
    }

}
