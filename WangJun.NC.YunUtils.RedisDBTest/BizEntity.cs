using System;
using System.Collections.Generic;
using System.Text;

namespace WangJun.NC.YunUtils.RedisDBTest
{
    public class BizEntity:Item
    {
        [Keyword]
        public string Title { get; set; }

        [Readonly]
        public DateTime CreateTime { get; set; }
         
        public DateTime UpdateTime { get; set; }

        public int Sort { get; set; }
    }
}
