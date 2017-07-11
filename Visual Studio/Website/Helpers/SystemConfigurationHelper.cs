using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Website.Helpers
{
    public class SystemConfigurationHelper
    {

        /// <summary>
        /// Set or get database connection string
        /// </summary>
        public static string Database_ConnectionString
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["Database:ConnectionString"].ConnectionString;
            }
            set
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings["Database:ConnectionString"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}