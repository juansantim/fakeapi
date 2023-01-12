using ProtectedApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProtectedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin_role")]
    //[Authorize(Roles = "consumer_role")]
    public class ProductsController : ControllerBase
    {

        [HttpGet(Name = "GetAllProducts")]
        public List<Product> GetAllProducts() 
        {
            List<Product> list = new List<Product>();

            for (int i = 1; i < 50; i++)
            {
                list.Add(new Product
                {
                    Id = i,
                    Name = Faker.Internet.UserName()
                });
            }

            return list;
        }
    }


}
