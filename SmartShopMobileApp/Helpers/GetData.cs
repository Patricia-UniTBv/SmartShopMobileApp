using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers
{
    public class GetData : ManageDataStrategy
    {
        public override async Task<string> ManageData(HttpClient httpClient, string endPoint, string json)
        {
            var uri = new Uri(Uri + endPoint);
            try
            {
                var response = await httpClient.GetAsync(uri);

                if (!response.IsSuccessStatusCode) return string.Empty;

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
