using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeRepository _empRepo;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly ILogger _logger;

        public HomeController(IEmployeeRepository empRepo,  
            IHostingEnvironment hostingEnv,
            ILogger<HomeController> logger)
        {
           _empRepo = empRepo;
            _hostingEnv = hostingEnv;
            _logger = logger;
        }
        
        [AllowAnonymous]
        public ViewResult Index()
        {  
            var model = _empRepo.GetAllEmployees();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            //throw new Exception("Error in Details");

            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");


            Employee emp = _empRepo.GetEmployee(id.Value);
            if(emp == null)
            {
                Response.StatusCode = 404;
                return View("EmpNotFound", id.Value);
            }
            HomeDetailsViewModel detailsViewModel = new HomeDetailsViewModel()
            {
                Employee = emp,
                PageTitle = "Employee Details"
            };
            
            return View(detailsViewModel);
        }

        [HttpGet]       
        public ViewResult Create()
        {
           return View();
        }

        [HttpPost]       
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                Employee newEmp = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Dept = model.Dept,
                    PhotoPath = uniqueFileName
                };
                _empRepo.Add(newEmp);
                return RedirectToAction("details", new { id = newEmp.Id });
            }
            return View();
        }

        [HttpGet]        
        public ViewResult Edit(int id)
        {
            Employee emp = _empRepo.GetEmployee(id);
            EmployeeEditViewModel editViewModel = new EmployeeEditViewModel()
            {
                Id = emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                Dept = emp.Dept,
                ExistingPhotoPath = emp.PhotoPath
            };
            return View(editViewModel);
        }
        
        [HttpPost]        
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee emp = _empRepo.GetEmployee(model.Id);
                emp.Name = model.Name;
                emp.Email = model.Email;
                emp.Dept = model.Dept;
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null) 
                    {
                        string filePath = Path.Combine(_hostingEnv.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    emp.PhotoPath = ProcessUploadedFile(model);
                };
                
                _empRepo.Update(emp);
                return RedirectToAction("index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
           _empRepo.Delete(id);
            
            return  RedirectToAction("index");
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadFolders = Path.Combine(_hostingEnv.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolders, uniqueFileName);

                using (var steam = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(steam);
                }
            
            }

            return uniqueFileName;
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        } 
    }
}
