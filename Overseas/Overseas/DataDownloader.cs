using System;
using System.Net;
using Json;
using Newtonsoft;


namespace Overseas
{
    class DataDownloader
    {
        private string brojPosiljke;

        public DataDownloader(string brojPosiljke)
        {
            this.brojPosiljke = brojPosiljke;
        }

        public JsonResponse getData()
        {
            string url = "https://my.overseas.hr/system/api/track-and-trace/get-shipment-data/" + this.brojPosiljke;
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance
                JsonResponse jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(json_data);
                return jsonResponse;
                //return !string.IsNullOrEmpty(json_data) ? Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(json_data) : new JsonResponse();
            }
        }
    }
}
