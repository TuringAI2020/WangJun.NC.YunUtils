using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public partial class DB : DbContext
    { 
        public DB()
        { 
        }

        public DB(DbContextOptions<DB> options)
            : base(options)
        {
        }
         
        public virtual DbSet<T> GetTable<T>() where T:class
        { 
                var set = this.Set<T>();
                return set; 
        }

        public bool Save<T>(T t, Action<T, T> callback = null, object[] keys =null) where T : class
        {
            var table = this.GetTable<T>();
            T valDB = null;
            if (keys != null&&0<keys.Length)
            {
                valDB= table.Find(keys);
            }

            if (null == valDB)
            {
                table.Add(t);
            }
            else
            {
                if (null != callback)
                {
                    callback(valDB, t);
                }
                table.Update(valDB);
            }

            var res = this.SaveChanges();
            return 0 < res;
        } 

        public bool Remove<T>(object[] keys) where T : class
        {
            if (null == keys || 0 == keys.Length)
            {
                return false;
            }

            var table = this.GetTable<T>();
            var valDB = table.Find(keys); 
            if (null != valDB)
            {
                table.Remove(valDB);
                var res = this.SaveChanges();
                return 0 < res;
            }
            return false;
        }

        public IQueryable<T> QueryList<T>(Expression<Func<T,bool>> query) where T : class
        { 
            var table = this.GetTable<T>();
            if (null != query)
            {
                var queryRes = table.Where(query);
                return queryRes;
            }
            return table.Where(p=>1==1);
        }

        public T QueryItem<T>(object[] keys) where T : class
        {
            var table = this.GetTable<T>();
            if (null != keys)
            {
                var queryRes = table.Find(keys);
                return queryRes;
            }
            return null;
        }
         

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
