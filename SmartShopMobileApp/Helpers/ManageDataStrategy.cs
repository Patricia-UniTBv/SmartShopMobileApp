using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers
{
    public abstract class ManageDataStrategy
    {
        //protected string Uri = "https://10.0.2.2:7116/api/";
        //protected string Uri = "https://localhost:7116/api/";
        public static string Uri = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7116/api/" : "https://localhost:5001";

        public abstract Task<string> ManageData(HttpClient httpClient, string endPoint, string json);
    }
}
