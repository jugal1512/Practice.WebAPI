using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.RegisterUser
{
    public class Login
    {
        [Required (ErrorMessage = "User Name is Required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }
    }
}
