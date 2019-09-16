using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace hmbzbfwfc.Models
{
    public static class AppSetting
    {

        public static string UploadUrl = (UploadUrl).getString();

        public static string sign = (sign).getString();

        public static string getString(this string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public static int getInt(this string key)
        {
            int num = 0;
            int.TryParse(ConfigurationManager.AppSettings[key], out num);
            return num;
        }
    }
}