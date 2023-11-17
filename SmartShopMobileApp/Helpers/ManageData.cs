using DTO;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers
{
    public class ManageData : IManageData
    {
        private ManageDataStrategy _manageDataStrategy;
        private HttpClient _httpClient;

        public void SetStrategy(ManageDataStrategy manageDataStrategy)
        {
            _manageDataStrategy = manageDataStrategy;
        }

        public async Task<T> GetDataAndDeserializeIt<T>(string url, string json)
        {
            try
            { 

                var _httpClient = new HttpClient(App.Current.MainPage.Handler.MauiContext.Services.GetService<IHttpClientHandlerService>().GetInsecureHandler());

                var data = await _manageDataStrategy.ManageData(_httpClient, url, json);

                if (data == string.Empty)
                    throw new TaskCanceledException();

                var deserializedData = JsonConvert.DeserializeObject<T>(data);
                if (deserializedData == null)
                {
                    throw new NotImplementedException();
                }
                return deserializedData;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
    }
}
