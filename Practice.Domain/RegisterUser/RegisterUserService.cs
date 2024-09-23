using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.RegisterUser
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IRegisterUserRepository _registerUserRepository;
        public RegisterUserService(IRegisterUserRepository registerUserRepository)
        {
            _registerUserRepository = registerUserRepository;
        }

        public async Task<IdentityUser> UserExists(string userName)
        {
            return await _registerUserRepository.UserExists(userName);
        }
        public async Task<IdentityResult> RegisterUser(RegisterUser user)
        {
            return await _registerUserRepository.RegisterUser(user);
        }

        public async Task<IdentityResult> AddToRole(RegisterUser user, string roleName)
        {
            return await _registerUserRepository.AddToRole(user, roleName);
        }
    }
}