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
            string request = "getApiKey";
            NameValueCollection data = new NameValueCollection();
            data["username"] = this.username;
            data["password"] = this.password;
            data["getApiKey"] = "";

            PostRequest postRequest = new PostRequest(request, data);

            JObject jsonResponse = postRequest.sendPostRequest();

            if(jsonResponse == null)
            {
                return null;
            }

            this.apiKey = jsonResponse["apiKey"].ToString();
            return this.apiKey;
        }
    }


}
