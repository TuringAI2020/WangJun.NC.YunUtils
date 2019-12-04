using System;
using System.Collections.Generic;
using System.Text;

namespace WangJun.NC.YunUtils
{
    public class Utils
    {
        #region Base64
        public RES ToBase64String(string input)
        {
            return RES.OK(BASE64.ToBase64String(input));
        }

        public RES FromBase64String(string input)
        {
            return RES.OK(BASE64.FromBase64String(input));
        }
        #endregion

        #region GUID
        public RES IsGuid(string input)
        {
            return RES.OK(GUID.IsGuid(input));
        }

        public RES FromStringToGuid(string input)
        {
            return RES.OK(GUID.FromStringToGuid(input));
        }
        #endregion

        #region MD5
        public RES ToMD5(string input)
        {
            return RES.OK(MD5.ToMD5(input));
        }

        public RES ToMD5_16(string input)
        {
            return RES.OK(MD5.ToMD5_16(input));
        }
        #endregion

        #region Redis
        public RES RedisExecute(string connection, string command, object[] paramArr)
        {
            if (!string.IsNullOrWhiteSpace(connection))
            {
                return RES.OK(REDIS.GetInst(0, connection).Execute(null, command, paramArr));
            }
            else
            {
                return RES.OK(REDIS.Current.Execute(null, command, paramArr));
            }
        }
        #endregion

        #region SHA1
        public RES ToSHA1(string input)
        {
            return RES.OK(SHA1.ToSHA1(input));
        }
        #endregion

        #region ID
        public static RES GetNewGuidList(string count)
        {
            return RES.OK(ID.GetNewGuidList(int.Parse(count)));
        }

        public static RES GetOrderIDList(string count)
        {
            return RES.OK(ID.GetOrderIDList(int.Parse(count)));
        }

        public static RES GetShortOrderIDList(string count)
        {
            return RES.OK(ID.GetShortOrderIDList(int.Parse(count)));
        }
        #endregion
    }
}
