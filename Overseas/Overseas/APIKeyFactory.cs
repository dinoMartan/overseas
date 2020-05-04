using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Overseas

{
    class APIKeyFactory
    {
        private string username;
        private string password;
        private string url = "http://moje-puzzle.com/API/request.php?getApiKey&";
        private string apiKey;

        public APIKeyFactory(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string getApikey()
        {
            string urlPost = "http://moje-puzzle.com/API/request.php";

            using (var wb = new WebClient())
            {               
                var data = new NameValueCollection();
                data["username"] = this.username;
                data["password"] = this.password;
                data["getApiKey"] = "";
                
                try
                {
                    var response = wb.UploadValues(urlPost, "POST", data);
                    string responseString = Encoding.UTF8.GetString(response);
                    JObject json = JObject.Parse(responseString);
                    this.apiKey = json["apiKey"].ToString();
                }

                catch(WebException e)
                {
                    //Alert.showAlert("Greška", e.Message);
                    return null;
                }                     
            }

            return this.apiKey;
        }
    }


}
