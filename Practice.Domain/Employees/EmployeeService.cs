using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Domain.Employees
{
    public class EmployeeService:IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _employeeRepository.GetAllEmployees();
        }
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            return await _employeeRepository.CreateEmployee(employee);
        }
    }
}
