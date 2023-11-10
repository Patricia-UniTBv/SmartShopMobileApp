using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace SmartShopMobileApp.Helpers
{

    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
