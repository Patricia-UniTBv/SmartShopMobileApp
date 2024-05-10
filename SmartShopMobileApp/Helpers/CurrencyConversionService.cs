using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers
{
    public class CurrencyConversionService
    {
        private readonly string _apiBaseUrl;
        private Dictionary<string, decimal> _currencies = new Dictionary<string, decimal>();


        public CurrencyConversionService()
        {
            var currencies = GetLatestCurrencies();
            dynamic responseObject = JsonConvert.DeserializeObject(currencies);

            if (responseObject != null && responseObject.data != null)
            {
                foreach (var item in responseObject.data)
                {
                    if (item.First != null && item.First.code != null)
                    {
                        string currencyCode = item.First.code.ToString();

                        if (item.First.value != null && item.First.value.Value != null)
                        {
                            decimal currencyValue;
                            if (decimal.TryParse(item.First.value.Value.ToString(), out currencyValue))
                            {
                                _currencies[currencyCode] = currencyValue;
                            }
                        }

                    }

                }
            }

        }

        private string GetLatestCurrencies()
        {
            //var client = new RestClient("https://api.currencyapi.com/v3/latest?base_currency=RON");

            //client.Timeout = -1;
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("apikey", "cur_live_wv1yJn5eStzk3YvzKRVLEnHkQsK2n6I2uzgfJkDx");
            //IRestResponse response = client.Execute(request);
            //return response.Content;
            return "";
        }


        public decimal ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            var sourceCurrencyExchangeRate = _currencies.First(c => c.Key == fromCurrency).Value;
            var destinationCurrencyExchangeRate = _currencies.First(c => c.Key == toCurrency).Value;
            var calculatedAmount = (amount * sourceCurrencyExchangeRate) * destinationCurrencyExchangeRate;
            return calculatedAmount;
        }

        public List<string> GetAllCurrencyCodes()
        {
            return _currencies.Keys.ToList();
        }
    }

}
