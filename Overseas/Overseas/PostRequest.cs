using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Overseas
{
    class PostRequest
    {
        private string request;
        private NameValueCollection data;

        private Dictionary<string, string> requestsUrls = new Dictionary<string, string>() {
            {"getApiKey", "http://moje-puzzle.com/API/request.php" }
        };

        public PostRequest(string request, NameValueCollection data)
        {
            this.request = request;
            this.data = data;       
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

            JObject jsonResponse;
            

            using (var wb = new WebClient())
            {
                try
                {
                    var response = wb.UploadValues(urlPost, "POST", this.data);
                    string responseString = Encoding.UTF8.GetString(response);
                    jsonResponse = JObject.Parse(responseString);
                }

                catch (WebException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            return jsonResponse;

        }

    }
}
