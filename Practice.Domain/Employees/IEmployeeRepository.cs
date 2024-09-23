using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Employees
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> GetAllEmployees();
        public Task<Employee> CreateEmployee(Employee employee);
        public Task<Employee> GetByEmployeeId(int id);
        public Task<Employee> UpdateEmployee(Employee employee);
        public Task<Employee> DeleteEmployee(Employee employee);
        public Task<Employee> SearchEmployeeByName(string searchString);
    }
}
