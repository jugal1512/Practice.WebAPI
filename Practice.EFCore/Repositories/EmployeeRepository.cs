using Microsoft.EntityFrameworkCore;
using Practice.Domain.Employees;
using Practice.EFCore.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.EFCore.Repositories
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employees.AsNoTracking().Include(s => s.Skills).ToListAsync();
        }
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
    }
}
