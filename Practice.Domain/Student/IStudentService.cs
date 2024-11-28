using Practice.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Student;
public interface IStudentService
{
    public Task<List<Student>> CreateStudents(List<Student> student);
}
