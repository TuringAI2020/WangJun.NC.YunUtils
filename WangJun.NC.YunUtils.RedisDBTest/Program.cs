using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WangJun.NC.YunUtils.RedisDBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var db = new RedisDB();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int k = 0; k < 1; k++)
            {
                //var item = new BizEntity { CreateTime = DateTime.Now.AddMinutes(k).AddSeconds(k), ItemID = Guid.NewGuid(), Sort = k, Title = $"标题标题标题标题标题{k}" };
                //item.ClassFullName = item.GetType().FullName;
                //db.Save<BizEntity>(item);

                //var item = new BizEntity { CreateTime = DateTime.Now.AddMinutes(k).AddSeconds(k) , UpdateTime=DateTime.Now, ItemID = Guid.Parse("4a27ad9d-86c7-48b6-b25e-cf989559cbe2"), Sort = k, Title = $"已更改标题标题标题标题标题{k}" };
                //item.ClassFullName = item.GetType().FullName;
                //db.Save<BizEntity>(item);
            }

            //var item1 = new BizEntity { CreateTime = DateTime.Now.AddMinutes(100).AddSeconds(100), ItemID = Guid.NewGuid(), Sort = 100, Title = $"第一篇文章" };
            //item1.ClassFullName = item1.GetType().FullName;
            //db.Save<BizEntity>(item1);

            //var item2 = new BizEntity { CreateTime = DateTime.Now.AddMinutes(200).AddSeconds(200), ItemID = Guid.NewGuid(), Sort = 200, Title = $"第二篇新闻" };
            //item2.ClassFullName = item2.GetType().FullName;
            //db.Save<BizEntity>(item2);

            //var res = db.QueryList<BizEntity>(new Dictionary<string, object> { { "Title", "新闻" } });
            var inst = (object)new BizEntity();
            var type = inst.GetType();
            var del =(Func<string, string>) type.GetMethod("Test").CreateDelegate(typeof(Func<string,string>),inst);
            var res = del("Ok");
            watch.Stop();
            Console.WriteLine($"{watch.Elapsed} {watch.Elapsed / 1}");
            Console.ReadKey();
        }
    }
}
