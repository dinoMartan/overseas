using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Overseas
{
    class DataDownloader
    {
        private string brojPosiljke;
        private string username;
        private string password;
        private string apiKey;

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
            JsonResponse jsonResponse = new JsonResponse();
            HttpRequst httpRequst = new HttpRequst(request: "getShipmentByShipmentNumber", shipmentNumber: this.brojPosiljke);
            JObject response = httpRequst.sendGetRequest();

            try
            {
                jsonResponse = response.ToObject<JsonResponse>();
            }
            catch(Exception e)
            {
                return null;
            }
            
            return jsonResponse;
        }

        public List<JsonResponse> getAllShippmentsAPI()
        {
            List<JsonResponse> jsonResponses = new List<JsonResponse>();

            // dohvati api kljuc sa servera uz korisnicko ime i lozinku
            APIKeyFactory apiKeyFactory = new APIKeyFactory(this.username, this.password);      
            this.apiKey = apiKeyFactory.getApikey();

            if (string.IsNullOrEmpty(apiKey))
            {
                Alert.showAlert("Greška", "API ključ nije dohvaćen");
                return null;
            }

            Console.WriteLine("Api key (DataDownloader): " + apiKey);

            // TO DO: POST REQUEST TO API
            
            // pripremi parametre
            string request = "getApiKey";
            NameValueCollection data = new NameValueCollection();
            data["api-key"] = this.apiKey;

            HttpRequst httpRequst = new HttpRequst(request: request, data: data);
            JObject response = httpRequst.sendPostRequest();
            try
            {
                jsonResponses = response.ToObject<List<JsonResponse>>();
            }
            catch(Exception e)
            {
                return null;
            }


            return jsonResponses;
            

        }

    }
}
