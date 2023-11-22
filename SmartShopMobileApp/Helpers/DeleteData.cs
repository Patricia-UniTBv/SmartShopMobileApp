using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers
{
    public class DeleteData : ManageDataStrategy
    {
        public async override Task<string> ManageData(HttpClient httpClient, string endPoint, string json)
        {
            var uri = new Uri(Uri + endPoint);
            try
            {
                var response = await httpClient.DeleteAsync(uri);

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
