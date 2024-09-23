using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Practice.Domain.Role;
using Practice.EFCore.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.EFCore.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            return roles;
        }
        public async Task<bool> RoleExists(string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            return roleExists;
        }
        public async Task<IdentityResult> CreateRole(string role)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
            return roleResult;
        }
    }
}
