using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Practice.Domain.Student;
using Practice.WebAPI.Models;

namespace Practice.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ExcelImportController : Controller
{
    private readonly StudentService _studentService;
    private readonly IMapper _mapper;
    public ExcelImportController(StudentService studentService, IMapper mapper)
    {
        _studentService = studentService;
        _mapper = mapper;
    }

    [HttpPost("ReadDataFromExcel")]
    public async Task<IActionResult> ReadDataFromExcelAsync(IFormFile file)
    {
        try
        {
            var newFileName = Guid.NewGuid() + "_" + file.FileName;
            var directoryExist = Path.Combine(Directory.GetCurrentDirectory(), "ExcelSheet");
            Directory.CreateDirectory(directoryExist);
            var filePath = Path.Combine(directoryExist, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var studentsViewModel = new List<StudentViewModal>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var student = new StudentViewModal
                    {
                        Id = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                        Name = worksheet.Cells[row, 2].Value.ToString()
                    };
                    studentsViewModel.Add(student);
                }
            }
            var students = _mapper.Map<List<Student>>(studentsViewModel);
            return Ok(await _studentService.CreateStudents(students));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
