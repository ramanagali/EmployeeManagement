using Microsoft.AspNetCore.DataProtection;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _empList;

        public MockEmployeeRepository()
        {
            _empList = new List<Employee>()
            {
                new Employee(){ Id=1, Name="GVR", Dept = Dept.IT, Email="ramana.gali@gmail.com"},
                new Employee(){ Id=2, Name="Shanvi", Dept = Dept.HR, Email="shanvi.gali@gmail.com"},
                new Employee(){ Id=3, Name="Manasvi", Dept = Dept.Payroll, Email="manasvi.gali@gmail.com"}
            };
        }

        public Employee Add(Employee emp)
        {
            emp.Id = _empList.Max(e => e.Id) + 1;
            _empList.Add(emp);
            return emp;
        }

        public Employee Delete(int id)
        {
            Employee emp = _empList.FirstOrDefault(e => e.Id == id);

            if(emp != null)
                _empList.Remove(emp);

            return emp;
        }

        public Employee GetEmployee(int id)
        {
            return _empList.FirstOrDefault(e => e.Id == id);
        }

        public Employee Update(Employee empChanges)
        {
            Employee emp = _empList.FirstOrDefault(e => e.Id == empChanges.Id);
            if (emp != null)
            {
                emp.Name = empChanges.Name;
                emp.Email = empChanges.Email;
                emp.Dept = empChanges.Dept;
            }
            return emp;
        }

        IEnumerable<Employee> IEmployeeRepository.GetAllEmployees()
        {
            return _empList;
        }
    }
}