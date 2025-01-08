using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(DataContext context) : ControllerBase
    {
        //get all the users
        //http:localhost:5000/api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await context.Users.ToListAsync();
            return Ok(users);
        }

        //get user by id
        //http:localhost:5000/api/users/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
