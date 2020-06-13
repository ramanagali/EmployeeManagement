using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLEmployeeRepository> _logger;

        public SQLEmployeeRepository(AppDbContext context, ILogger<SQLEmployeeRepository> logger)
        {
            this.context = context;
            this._logger = logger;
        }
        public Employee Add(Employee emp)
        {
            context.Employees.Add(emp);
            context.SaveChanges();
            return emp;
        }

        public Employee Delete(int id)
        {
            Employee emp = context.Employees.Find(id);
            if (emp != null)
            {
                context.Employees.Remove(emp);
                context.SaveChanges();
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");

            return context.Employees.Find(id);
        }

        public Employee Update(Employee empChanges)
        {
            var emp = context.Employees.Attach(empChanges);
            emp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return empChanges;
        }
    }
}
