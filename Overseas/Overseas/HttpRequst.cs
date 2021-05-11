using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Overseas
{
    class HttpRequst
    {
        private string request;
        private NameValueCollection data;
        private JObject jsonResponse;
        private string shipmentNumber;

        private Dictionary<string, string> requestsUrls = new Dictionary<string, string>() {
            {"getApiKey", "http://moje-puzzle.com/API/request.php" },
            {"getShipmentByShipmentNumber", "https://my.overseas.hr/system/api/track-and-trace/get-shipment-data/" },
            {"GetShipmentStatusByShipmentId", "http://webapitest.overseas.hr/api/data/GetShipmentStatusByShipmentId" },
            { "GetActiveShipments", "http://webapitest.overseas.hr/api/data/GetActiveShipments" }
        };

        public HttpRequst(string request, NameValueCollection data = null, string shipmentNumber = "")
        {
            this.request = request;
            this.data = data;
            this.shipmentNumber = shipmentNumber;
        }

        // vraca null ako primi status kod != 200 ili ako ne postoji zahtjev
        public JObject sendPostRequest()
        {
            string urlPost;
            if (requestsUrls.ContainsKey(request))
            {
                urlPost = requestsUrls[request];
            }
            else
            {
                Alert.showAlert("Greška", "Ne postoji takav zahtjev!");
                return null;
            }
        
            using (var wb = new WebClient() { Encoding = Encoding.UTF8 })
            {
                try
                {
                    var response = wb.UploadValues(urlPost, "POST", this.data);
                    string responseString = Encoding.UTF8.GetString(response);
                    this.jsonResponse = JObject.Parse(responseString);
                }

                catch (WebException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            return this.jsonResponse;
        }

        public JObject sendGetRequest()
        {
            string urlGet;
            if (requestsUrls.ContainsKey(request))
            {
                urlGet = requestsUrls[request];
            }
            else
            {
                Alert.showAlert("Greška", "Ne postoji takav zahtjev!");
                return null;
            }

            if(this.shipmentNumber != "")
            {
                urlGet = urlGet += this.shipmentNumber;
            }
            else
            {
                // kreiranje url-a
                // u url se umecu vrijednosti kljuc,vrijednost iz data
                int i = 1;
                foreach (string key in data)
                {
                    string value = data[key];

                    if (i == 1) { urlGet += "?" + key + "=" + value; }
                    else { urlGet += "&" + key + "=" + value; }

                    i++;
                }
            }

            using (var wb = new WebClient() { Encoding = Encoding.UTF8 })
            {
                try
                {
                    var responseString = wb.DownloadString(urlGet);
                    this.jsonResponse = JObject.Parse(responseString);
                }
                catch(WebException e)
                {
                    Console.WriteLine("HttpRequest class - sendGetRequest - WebExcetion: " + e);
                    return null;
                }
            }

            return this.jsonResponse;
        }

    }
}
