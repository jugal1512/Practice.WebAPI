using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Role
{
    public interface IRoleService
    {
        public Task<List<IdentityRole>> GetAllRoles();
        public Task<bool> RoleExists(string role);
        public Task<IdentityResult> CreateRole(string role);
    }
}
