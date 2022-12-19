using FakeApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ClientsController : ControllerBase
    {

        [HttpGet(Name = "GetWeatherForecast")]
        public List<Client> GetAllClients() 
        {
            List<Client> clients = new List<Client>();

            for (int i = 1; i < 50; i++)
            {
                clients.Add(new Client
                {
                    Id = i,
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    Addres = Faker.Address.UsTerritory(),
                    Company = Faker.Company.Name(),
                    PhoneNumber = Faker.Phone.Number(),
                });
            }

            return clients;
        }
    }
}
