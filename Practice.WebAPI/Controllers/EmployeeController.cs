using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Practice.Domain.Employees;
using Practice.Domain.Skills;
using Practice.WebAPI.Models;

namespace Practice.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EmployeeService _employeeService;
        private readonly SkillService _skillService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IMapper mapper, EmployeeService employeeService, SkillService skillService , IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _skillService = skillService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployees();
                if (employees.Count() > 0)
                {
                    List<EmployeeDto> employee = new List<EmployeeDto>();
                    foreach (var item in employees)
                    {
                        EmployeeDto employeeDto = new EmployeeDto();
                        employeeDto.Id = item.Id;
                        employeeDto.FirstName = item.FirstName;
                        employeeDto.LastName = item.LastName;
                        employeeDto.Email = item.Email;
                        employeeDto.Phone = item.Phone;
                        employeeDto.Gender = item.Gender;
                        employeeDto.Designation = item.Designation;
                        employeeDto.ProfileImageName = item.ProfileImage;
                        employeeDto.Address = item.Address;
                        List<SkillDto> skillItems = new List<SkillDto>();
                        foreach (var skillItem in item.Skills)
                        {
                            SkillDto skill = new SkillDto();
                            skill.SkillId = skillItem.SkillId;
                            skill.SkillName = skillItem.SkillName;
                            skill.EmployeeId = skillItem.EmployeeId;
                            skillItems.Add(skill);
                        }
                        employeeDto.Skills = skillItems;
                        employee.Add(employeeDto);
                    }
                    return Ok(employee);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Employees Details Not Found!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeDto employeeDto)
        {
            try
            {
                string profileImageName = await SaveProfileImage(employeeDto.ProfileImage);

                var skillItems = ParseSkills(employeeDto.SkillName);

                var employeeModal = _mapper.Map<Employee>(employeeDto);
                employeeModal.Skills = skillItems;
                employeeModal.ProfileImage = profileImageName;
                var employee = await _employeeService.CreateEmployee(employeeModal);
                return Ok(new Response { Status = "Success", Message = "Employee Create Successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromForm]EmployeeDto employee)
        {
            try 
            {
                if (!string.IsNullOrEmpty(employee.SkillName))
                {
                    await _skillService.DeleteSkills(employee.Id);
                }
                var skillItems = ParseSkills(employee.SkillName);

                var employeeExist = await _employeeService.GetByEmployeeId(employee.Id);
                string profileImageName = employeeExist.ProfileImage;
                if (employee.ProfileImage != null)
                {
                    if (employeeExist.ProfileImage != null)
                    {
                        DeleteOldProfileImage(employeeExist.ProfileImage);
                    }
                    profileImageName = await SaveProfileImage(employee.ProfileImage); ;
                }
                var updateEmployeeDetails = _mapper.Map<Employee>(employee);
                updateEmployeeDetails.Skills = skillItems;
                updateEmployeeDetails.ProfileImage = profileImageName;
                var updateEmployee = await _employeeService.UpdateEmployee(updateEmployeeDetails);
                return Ok(new Response { Status = "Success", Message = "Employee Update Successfully." });
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        private async Task<string> SaveProfileImage(IFormFile profileImage)
        {
            var newFileName = Guid.NewGuid() + "_" + profileImage.FileName;
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "ProfileImages");
            Directory.CreateDirectory(directoryPath);
            var filePath = Path.Combine(directoryPath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            return newFileName;
        }

        private void DeleteOldProfileImage(string profileImage)
        {
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "ProfileImages", profileImage);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        private List<Skill> ParseSkills(string skillNames)
        {
            var skills = new List<Skill>();
            if (!string.IsNullOrEmpty(skillNames))
            {
                var skillArray = skillNames.Split(",");
                skills.AddRange(skillArray.Select(name => new Skill { SkillName = name.Trim() }));
            }
            return skills;
        }
    }
}
