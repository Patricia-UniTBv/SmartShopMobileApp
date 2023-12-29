using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers
{
    public abstract class ManageDataStrategy
    {
        public static string Uri = "https://smartshopwebapi.azurewebsites.net/api/";
        //public static string Uri = "https://10.0.2.2:7116/api/";

        public abstract Task<string> ManageData(HttpClient httpClient, string endPoint, string json);
    }
}
