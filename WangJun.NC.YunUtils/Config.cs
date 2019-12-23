using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WangJun.NC.YunUtils
{
    static class Config
    {
        private static Dictionary<string, IConfiguration> all =new Dictionary<string, IConfiguration>();
        public static bool Load(string uri,string alias= "Default")
        {
            try
            {
                if (all.ContainsKey(alias))
                {
                    all.Remove(alias);
                }
                else
                {
                    IConfiguration icfg = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = uri, ReloadOnChange = true }).Build();
                    all.Add(alias, icfg);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static IConfiguration Default
        {
            get
            {
                var alias = "Default";
                if (all.ContainsKey(alias))
                {
                    return all[alias];
                }
                return null;
            }
        }

        public static IConfiguration GetConfiguration(string alias = "Default")
        { 
            if (all.ContainsKey(alias))
            {
                return all[alias];
            }
            return null;
        }

        public static string GetConnection(string name, string alias = "Default")
        {
            var cfg = GetConfiguration(alias);
            if (null != cfg)
            {
                var val = cfg.GetConnectionString(name);
                return val;
            }
            return null;
        }

        public static string GetAppSetting(string name, string alias = "Default")
        {
            var cfg = GetConfiguration(alias);
            if (null != cfg)
            {
                var sec = cfg.GetSection(name);
                
                return sec.Value;
            }
            return null;
        }
    }
}
