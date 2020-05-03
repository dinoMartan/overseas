using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Overseas

{
    class APIKeyFactory
    {
        private string url = "http://moje-puzzle.com/API/request.php?getApiKey&";
        private string apiKey;

        public APIKeyFactory(string username, string password)
        {
            this.url = this.url + "username=" + username + "&password=" + password;
        }

        public string getApikey()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;

                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    JObject json = JObject.Parse(responseString);
                    this.apiKey = json["apiKey"].ToString();
                }

                else
                {
                    Alert.showAlert("Greška", responseString);
                }
            }

            return this.apiKey;
        }
    }


}
