using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Core;
using User.Core.Attributes;

namespace User.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public List<string> getCategories()
        {
            return new List<string>()
           {
                new string("Category 1"),
                new string("Category 2"),
                new string("Category 3"),
           };
        }
    }
}
