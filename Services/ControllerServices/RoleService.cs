using ImHungryBackendER.Services.ControllerServices.Interfaces;
using ImHungryLibrary.Models;

namespace ImHungryBackendER.Services.ControllerServices
{
    public class RoleService : IRoleService
    {
        private readonly ImHungryContext _context;

        public RoleService(ImHungryContext context) {
            _context = context;
        }

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
