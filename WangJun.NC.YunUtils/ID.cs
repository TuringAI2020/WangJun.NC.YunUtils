using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WangJun.NC.YunUtils
{
    public static class ID
    {
        public static Guid GUID
        {
            get
            {
                return Guid.NewGuid();
            }
        }

        private static ulong timeStart = 0;
        private static ulong MS
        {
            get
            {
                var startVal = ulong.Parse(DateTime.Now.ToString("yyyyMMddhhmmssfff")) * 100;
                if (timeStart < startVal)
                {
                    timeStart = startVal;
                }
                else if (timeStart <= (startVal + 1000))
                {
                    timeStart++;
                }
                else
                {
                    Thread.Sleep(1);
                }
                return timeStart;
            }
        }

        private static ulong MsSpan
        {
            get
            {
                var msSub = ulong.Parse(DateTime.Parse("2000-1-1").ToString("yyyyMMddhhmmssfff")) * 100;
                return MS - msSub;
            }
        }
        public static string AppID(string appSecret)
        { 
                var str = MD5.ToMD5_16(appSecret);
                return str; 
        }

        public static string AppSecret
        {
            get
            {
                return OrderID;
            }
        }

        public static string OrderID
        {
            get
            { 
                return ID.MS.ToString();
            }
        }

        public static string ShortOrderID
        {
            get
            {
                return ID.MsSpan.ToString();
            }
        }

        public static List<string> GetNewGuidList(int count)
        {
            var list = new List<string>();
            count = Math.Abs(count) % 1001;
            for (int k = 0; k < count; k++)
            {
                list.Add(Guid.NewGuid().ToString());
            }
            return list;
        }

        public static List<string> GetOrderIDList(int count)
        {
            var list = new List<string>();
            count = Math.Abs(count) % 101;
            for (int k = 0; k < count; k++)
            {
                list.Add(ID.OrderID);
            }
            return list;
        }

        public static List<string> GetShortOrderIDList(int count)
        {
            var list = new List<string>();
            count = Math.Abs(count) % 101;
            for (int k = 0; k < count; k++)
            {
                list.Add(ID.ShortOrderID);
            }
            return list;
        }

    }
}
