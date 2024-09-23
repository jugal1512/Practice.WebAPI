using Microsoft.EntityFrameworkCore;
using Practice.Domain.Employees;
using Practice.EFCore.DBContext;

namespace Practice.EFCore.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
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

        public async Task<Employee> GetByEmployeeId(int id)
        {
            var employee =  await _context.Employees.Where(e => e.Id == id).AsNoTracking().FirstOrDefaultAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var employeeExist = await GetByEmployeeId(employee.Id);
            if (employeeExist != null) {
                var updateEmployee = _context.Update(employee);
                _context.SaveChanges();
            }
            return employee;

        }

        public Task<Employee> DeleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> SearchEmployeeByName(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
