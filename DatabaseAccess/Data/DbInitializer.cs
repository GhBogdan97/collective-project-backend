using DatabaseAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            if (await context.Roles.AnyAsync())
            {
                return;
            }

            await Seed(context, userManager, roleManager);
        }


        public static async Task Seed(ApplicationDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            await userManager.CreateAsync(new ApplicationUser { Email = "simonaburlacu01@gmail.com", UserName = "simonaburlacu01@gmail.com" }, "Password123.");
            await roleManager.CreateAsync(new IdentityRole { Name = "Student" });
            var student = await userManager.FindByEmailAsync("simonaburlacu01@gmail.com");
            var roleStudent = await roleManager.FindByNameAsync("Student");
            await userManager.AddToRoleAsync(student, roleStudent.Name);

            Student studentDb = new Student()
            {
                Firstname = "Simona",
                Lastname = "Burlacu",
                University = "UBB",
                College = "Facultatea de Matematica-Informatica",
                Specialization = "Informatica-Romana",
                Year = 3,
                IdUser = student.Id
            };

            context.Students.Add(studentDb);
            context.SaveChanges();
         
        }

    }
}
