using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartShopMobileApp;

//[assembly: Dependency(typeof(SmartShopMobileApp.Helpers.HttpClientHandlerService))]
namespace SmartShopMobileApp.Helpers
{
    public class HttpClientHandlerService : IHttpClientHandlerService
    {
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;

                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}
