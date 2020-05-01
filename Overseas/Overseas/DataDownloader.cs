using System;
using System.Net;
using System.Text;

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
            //string url = "https://my.overseas.hr/system/api/track-and-trace/get-shipment-data/07074954";

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
                // if string with JSON data is not empty, deserialize it to class and return its instance
                JsonResponse jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(json_data);
                return jsonResponse;
                //return !string.IsNullOrEmpty(json_data) ? Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(json_data) : new JsonResponse();
            }
        }
    }
}
