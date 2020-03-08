using System;
using System.Collections.Generic;
using System.Text;

namespace WangJun.NC.YunUtils
{
    public class QueryFilter
    {

        public string ID { get; set; }
        public string Keywords { get; set; }

        public string CreatorId { get; set; }

        public string ParentId { get; set; }
        public string GroupId { get; set; }
        

        public bool CreateTimeDESC{ get; set; }

        public string Status { get; set; }

        public string AppId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            if (this.PageIndex <= 0) {
                this.PageIndex = 0;
            }

            if (this.PageSize <= 0)
            {
                this.PageSize = 10000;
            }
            var json = JSON.ToJson(this);
            var dict = JSON.ToObject<Dictionary<string, object>>(json);
            return dict;
        }

    }
}
