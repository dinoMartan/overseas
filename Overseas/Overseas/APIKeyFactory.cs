using Newtonsoft.Json.Linq;
using System.Collections.Specialized;

namespace Overseas

{
    class APIKeyFactory
    {
        private string username;
        private string password;
        private string apiKey;

        public APIKeyFactory(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string getApikey()
        {
            // pripremi parametre
            string request = "getApiKey";
            NameValueCollection data = new NameValueCollection();
            data["username"] = this.username;
            data["password"] = this.password;
            data["getApiKey"] = "";

            // kreiraj httpRequest s parametrima
            HttpRequst httpRequest = new HttpRequst(request, data);

            // metoda sendPostRequest dohvaca odgovor kao JObject
            JObject jsonResponse = httpRequest.sendPostRequest();

            if(jsonResponse == null)
            {
                return null;
            }

            // vraca apiKey
            this.apiKey = jsonResponse["apiKey"].ToString();
            return this.apiKey;
        }
    }


}
