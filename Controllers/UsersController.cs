using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserMvcProject.Data;
using UserMvcProject.Models;
using UserMvcProject.Models.Domain;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;


namespace UserMvcProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public UsersController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
        //Searching
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)

        {
            var users = from u in mvcDemoDbContext.Users
                        select u;
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.City == searchString);
            }

            return View(await mvcDemoDbContext.Users.Where(u => u.City.Contains(searchString) || 
            searchString == null || u.FirstName.Contains(searchString) || u.LastName.Contains(searchString) ||
            u.Country.Contains(searchString) || u.Gender.Contains(searchString)).ToListAsync());
        }
        public async Task<IActionResult> Index1(string sortOrder)

        {
            ViewBag.FirstNameSortParm = string.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : "";
            ViewBag.LastNameSortParm = sortOrder== "Lastname" ? "LastName_desc" : "";
            //ViewBag.CitySortParm = string.IsNullOrEmpty(sortOrder) ? "City_desc" : "";
            
            var users = from u in mvcDemoDbContext.Users.ToList()
                        select u;
            switch(sortOrder)
            {
                case "FirstName_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;

                case "LastName":
                    users = users.OrderBy(u => u.LastName);
                    break;

                case "LastName_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;

                default:
                    users = users.OrderBy(u => u.FirstName);
                    break;
            

            }
            return View(await mvcDemoDbContext.Users.ToListAsync());


        }

        /*ViewData["sortNameParameter"] = string.IsNullOrEmpty(sortBy) ? "namedesc" : "";

        ViewData["sortCityParameter"] = string.IsNullOrEmpty(sortBy) ? "City_desc" : "";

        var users = from u in mvcDemoDbContext.Users
            select u;
        switch (sortBy)
        {
            case "namedesc":
                users = users.OrderByDescending(u => u.FirstName);
                break;
            default:
                break;*/






        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add( AddUserViewModel addUserRequest)
        { 

        var user = new User()


            {

                FirstName = addUserRequest.FirstName,
                LastName = addUserRequest.LastName,
                City = addUserRequest.City,
                Country = addUserRequest.Country,
                Gender = addUserRequest.Gender,
                Status = addUserRequest.Status
            };

            await mvcDemoDbContext.Users.AddAsync(user);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
