using System.ComponentModel.DataAnnotations.Schema;

namespace Practice.WebAPI.Models
{
    public class SkillDto
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
