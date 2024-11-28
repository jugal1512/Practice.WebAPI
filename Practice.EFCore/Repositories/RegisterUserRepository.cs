using Microsoft.AspNetCore.Identity;
using Practice.Domain.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.EFCore.Repositories
{
    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterUserRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
              

        public async Task<IdentityUser> UserExists(string userName)
        {
            var userExists = await _userManager.FindByNameAsync(userName);
            return userExists;
        }
        public async Task<IdentityResult> RegisterUser(RegisterUser user)
        {
            IdentityUser addUser = new()
            {
                Email = user.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = user.UserName,
                TwoFactorEnabled = true,
            };

            var userResult = await _userManager.CreateAsync(addUser, user.Password);
            return userResult;
        }

        public async Task<IdentityResult> AddToRole(RegisterUser user, string roleName)
        {
            var existingUser = await UserExists(user.UserName);
            var addToRole = await _userManager.AddToRoleAsync(existingUser, roleName);
            return addToRole;
        }   
    }
}