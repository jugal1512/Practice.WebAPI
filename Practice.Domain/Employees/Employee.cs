using Practice.Domain.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Employees
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public string ProfileImage { get; set; }
        public string Address { get; set; }
        public List<Skill> Skills { get; set; }
    }
}
