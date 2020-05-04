using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Overseas
{
    class DataDownloader
    {
        private string brojPosiljke;
        private string username;
        private string password;

        public DataDownloader(string brojPosiljke)
        {
            this.brojPosiljke = brojPosiljke;
        }

        public DataDownloader(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        // vraca objekt JsonReponse
        public JsonResponse getData()
        {
            string url = "https://my.overseas.hr/system/api/track-and-trace/get-shipment-data/" + this.brojPosiljke;

            using (var w = new WebClient() { Encoding = Encoding.UTF8})
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                // deserialize json string to class and return its instance
                JsonResponse jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(json_data);
                return jsonResponse;
            }
        }

        public List<JsonResponse> getAllShippmentsAPI()
        {
            APIKeyFactory apiKeyFactory = new APIKeyFactory(this.username, this.password);
            List<JsonResponse> jsonResponses = new List<JsonResponse>();
            string apiKey = apiKeyFactory.getApikey();

            if (string.IsNullOrEmpty(apiKey))
            {
                Alert.showAlert("Greška", "API ključ nije dohvaćen");
                return null;
            }

            Console.WriteLine("Api key (DataDownloader): " + apiKey);

            // TO DO: POST REQUEST TO API
            

            string url = "https://my.overseas.hr/system/api/track-and-trace/get-shipment-data/" + this.brojPosiljke;
            
            /*
            string result;

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = client.GetAsync(apiKey).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
            }

            // deserialize json string to class and return its instance
            jsonResponses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JsonResponse>>(result);

            */

            return jsonResponses;
            

        }

    }
}
