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

        public ResultSet Get(string managedIdentityClientId = "e1ff786b-419f-42b4-9678-ae50292faed9",
            string appId = "ae4aeb26-4d3e-4d33-9daf-f58ea6f262dd")
        {
            ResultSet result = new ResultSet();

            HttpClient client = new HttpClient();
            string token = string.Empty;
            string clientId = string.Empty;

            try
            {
                (token, clientId) = GenerateToken(managedIdentityClientId, appId);
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

        //[HttpGet(Name = "GetToken")]
        //public string GetToken(string managedIdentityClientId = "e1ff786b-419f-42b4-9678-ae50292faed9",
        //    string appId = "ae4aeb26-4d3e-4d33-9daf-f58ea6f262dd")
        //{
        //    var (token, _) = GenerateToken(managedIdentityClientId, appId);

        //    return token;
        //}

        private (string token, string clientID) GenerateToken(string managedIdentityClientId,string appId)
        {
            DefaultAzureCredential credential = string.IsNullOrEmpty(managedIdentityClientId) ? new DefaultAzureCredential() :
                new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId });

            var context = new TokenRequestContext(new string[] { $"{appId}/.default" });

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