using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.RegisterUser
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "User Name is Required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }

        [Required]
        public string RoleName { get; set; }
        [Required]
        public bool TwoFactorAuthenticate { get; set; }
    }
}