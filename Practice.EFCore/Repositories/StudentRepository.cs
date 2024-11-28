using Practice.Domain.Employees;
using Practice.Domain.Student;
using Practice.EFCore.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.EFCore.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;
    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Student>> CreateStudents(List<Student> student)
    {
        _context.Students.AddRange(student);
        await _context.SaveChangesAsync();
        return student;
    }
}
