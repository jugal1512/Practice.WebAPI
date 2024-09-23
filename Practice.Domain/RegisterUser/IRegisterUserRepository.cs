using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.RegisterUser
{
    public interface IRegisterUserRepository
    {
        public Task<IdentityUser> UserExists(string userName);
        public Task<IdentityResult> RegisterUser(RegisterUser user);
        public Task<IdentityResult> AddToRole(RegisterUser user, string roleName);
    }
}
