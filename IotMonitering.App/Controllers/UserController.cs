using IoTMonitering.Domain.Entity;
using IoTMonitoring.Data;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private AppDbContext _context;

        public UserController(AppDbContext appContext)
        {
            _context = appContext;
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            string id = Guid.NewGuid().ToString();
            var newUser = new User()
            {
                UserID = id
            };
            _context.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { id = id});
        }
    }
}
