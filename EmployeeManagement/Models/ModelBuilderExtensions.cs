using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 1,
                   Name = "GVR",
                   Dept = Dept.IT,
                   Email = "gvr@gvr.comm"
               },
               new Employee
               {
                   Id = 2,
                   Name = "Mary",
                   Dept = Dept.HR,
                   Email = "mary@gvr.com"
               }
            );
        }
    }
}
