using ImHungryLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RoleController : Controller
    {
        private readonly ImHungryContext _context;

        public RoleController(ImHungryContext context)
        {
            _context = context;
        }

        [HttpPost("AddRole")]
        public async Task AddRole(string roleName)
        {
            var newRole = new Role()
            {
                RoleName = roleName
            };

            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync();
        }
    }
}
