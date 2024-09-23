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

        public async Task<Employee> GetByEmployeeId(int id)
        {
            return await _employeeRepository.GetByEmployeeId(id);
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            return await _employeeRepository.UpdateEmployee(employee);
        }

        public async Task<Employee> DeleteEmployee(Employee employee)
        {
            return await _employeeRepository.DeleteEmployee(employee);
        }

        public async Task<Employee> SearchEmployeeByName(string searchString)
        {
            return await _employeeRepository.SearchEmployeeByName(searchString);
        }
    }
}
