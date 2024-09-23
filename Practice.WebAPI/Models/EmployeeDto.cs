namespace Practice.WebAPI.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string? ProfileImageName { get; set; }
        public string Address { get; set; }
        public string SkillName { get; set; }
        public List<SkillDto>? Skills { get; set; }
    }
}
