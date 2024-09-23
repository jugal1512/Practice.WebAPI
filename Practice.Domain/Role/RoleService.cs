using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<List<IdentityRole>> GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
        }

        public Task<bool> RoleExists(string role)
        {
            return _roleRepository.RoleExists(role);
        }

        public async Task<IdentityResult> CreateRole(string role)
        {
            return await _roleRepository.CreateRole(role);
        }
    }
}
