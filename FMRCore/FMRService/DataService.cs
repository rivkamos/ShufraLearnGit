using System;
using System.Collections.Generic;
using System.Text;
using FMRCommon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FMRCore.FMRService
{
    public class DataService : IDataService
    {
        public List<Stock> GetStocks()
        {
            try
            {
                var client = new RestClient("https://api.tase.co.il/api/marketdata/etfs");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(new { dType = 1, TotalRec = 1, pageNum = 1, lang = 0 });
                var response = client.Execute(request);

                var o = JsonConvert.DeserializeObject<JObject>(response.Content);
                List<Stock> data = o.Value<JArray>("Items")
                    .ToObject<List<Stock>>();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
