using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IMapper mapper, EmployeeService employeeService, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _employeeService = employeeService;
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


        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeDto employeeDto)
        {
            try
            {
                var newFileName = Guid.NewGuid().ToString() + "_" + employeeDto.ProfileImage.FileName;
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "ProfileImages");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var filePath = Path.Combine(directoryPath, newFileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await employeeDto.ProfileImage.CopyToAsync(stream);
                }

                List<Skill> skillItems = new List<Skill>();
                if (!string.IsNullOrEmpty(employeeDto.SkillName))
                {
                    var skillArray = employeeDto.SkillName.Split(",");
                    if (skillArray.Length > 0)
                    {
                        foreach (var item in skillArray)
                        {
                            Skill skill = new Skill();
                            skill.SkillName = item;
                            skillItems.Add(skill);
                        }
                    }
                }

                var employeeModal = _mapper.Map<Employee>(employeeDto);
                employeeModal.Skills = skillItems;
                employeeModal.ProfileImage = newFileName;
                var employee = await _employeeService.CreateEmployee(employeeModal);
                return Ok(new Response { Status = "Success", Message = "Employee Create Successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
