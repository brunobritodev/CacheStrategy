using CacheStrategy.Stores;
using Microsoft.AspNetCore.Mvc;

namespace CacheStrategy.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserStore _store;

        public UserController(IUserStore store)
        {
            _store = store;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_store.List());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_store.Get(id));
        }
    }
}
