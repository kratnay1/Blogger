using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Blogger.Controllers
{
    public class CommonUtility
    {
        public static string GetMainConnectionstring()
        {
            return ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
        }
    }
}