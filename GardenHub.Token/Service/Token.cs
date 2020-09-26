using GardenHub.Token.Domain;
using Newtonsoft.Json;
using RestSharp;

namespace GardenHub.Token.Service
{
    public class Token
    {
        public static string Generate(string email, string password)
        {
            var client = new RestClient();

            var requestToken = new RestRequest("https://localhost:5003/api/authenticate/token");

            requestToken.AddJsonBody(JsonConvert.SerializeObject(new
            {
                Email = email,
                Password = password
            }));

            var result = client.Post<TokenResult>(requestToken).Data;

            //if (result == null) { return  }

            //var request = new RestRequest("https://localhost:5001/api/friend/", DataFormat.Json);
            //request.AddHeader("Authorization", "Bearer " + result.Token);

            return result.Token;
        }
    }
}
