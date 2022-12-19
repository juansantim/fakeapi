using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ConsumerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumerController : ControllerBase
    {

        [HttpGet(Name = "GetClients")]

        public ResultSet Get(string managedIdentityClientId)
        {
            ResultSet result = new ResultSet();

            HttpClient client = new HttpClient();
            string token = string.Empty;
            string clientId = string.Empty;

            try
            {
                (token, clientId) = GenerateToken(managedIdentityClientId);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var clients = client.GetAsync("https://jsprotectedapi.azurewebsites.net/api/Clients").Result;

                result.Result = clients.Content.ReadAsStringAsync().Result;
                result.Token = token;
                result.ClientId = clientId;
            }
            catch (Exception ex)
            {
                result.Result = ex.Message;
                result.Token = token;
            }

            return result;
        }

        private (string token, string clientID) GenerateToken(string managedIdentityClientId)
        {
            DefaultAzureCredential credential = string.IsNullOrEmpty(managedIdentityClientId) ? new DefaultAzureCredential() :
                new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId });

            var context = new TokenRequestContext(new string[] { "ae4aeb26-4d3e-4d33-9daf-f58ea6f262dd/.default" });

            var accesToken = credential.GetTokenAsync(context).Result;

            return (accesToken.Token, managedIdentityClientId);
        }

        public class ResultSet
        {
            public string? Result { get; set; }
            public string? Token { get; set; }
            public string? ClientId { get; set; }
        }
    }
}