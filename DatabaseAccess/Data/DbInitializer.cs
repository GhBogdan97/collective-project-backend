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


        public static async Task Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            await userManager.CreateAsync(new ApplicationUser { Email = "simonaburlacu01@gmail.com", UserName = "simonaburlacu01@gmail.com" }, "Password123.");
            await userManager.CreateAsync(new ApplicationUser { Email = "testStudent1@email.com", UserName = "testStudent1@email.com" }, "Password123.");
            await userManager.CreateAsync(new ApplicationUser { Email = "testCompanyUser1@email.com", UserName = "testCompanyUser1@email.com" }, "Password123.");
            await userManager.CreateAsync(new ApplicationUser { Email = "testCompanyUser2@email.com", UserName = "testCompanyUser2@email.com" }, "Password123.");

            await roleManager.CreateAsync(new IdentityRole { Name = "Student" });
            await roleManager.CreateAsync(new IdentityRole { Name = "Company" });

            var student1 = await userManager.FindByEmailAsync("simonaburlacu01@gmail.com");
            var student2 = await userManager.FindByEmailAsync("testStudent1@email.com");
            var company1 = await userManager.FindByEmailAsync("testCompanyUser1@email.com");
            var company2 = await userManager.FindByEmailAsync("testCompanyUser2@email.com");

            var roleStudent = await roleManager.FindByNameAsync("Student");
            var roleCompany = await roleManager.FindByNameAsync("Company");

            #region Set Roles
            await userManager.AddToRoleAsync(student1, roleStudent.Name);
            await userManager.AddToRoleAsync(student2, roleStudent.Name);
            await userManager.AddToRoleAsync(company1, roleCompany.Name);
            await userManager.AddToRoleAsync(company2, roleCompany.Name);
            #endregion

            List<Student> students = new List<Student> {
                new Student()
                {
                    Firstname = "Simona",
                    Lastname = "Burlacu",
                    University = "UBB",
                    College = "Facultatea de Matematica-Informatica",
                    Specialization = "Informatica-Romana",
                    Year = 3,
                    IdUser = student1.Id
                },
                new Student()
                {
                    Firstname = "Ionescu",
                    Lastname = "Agarbiceanu",
                    University = "UBB",
                    College = "Facultatea de Matematica-Informatica",
                    Specialization = "Informatica-Romana",
                    Year = 2,
                    IdUser = student2.Id
                }
            };

            List<Company> companies = new List<Company> {
                new Company
                {
                    Name = "Accesa", //pai da cum
                    Description = "O companie ca oricare alta",
                    Url = "www.google.com",
                    IdUser = company1.Id
                },
                 new Company()
                {
                    Name = "Yardi",
                    Description = "O companie",
                    Url = "www.yahoo.com",
                    IdUser = company2.Id
                 }
            };

            foreach (var stud in students)
            {
                context.Students.Add(stud);
            }
            context.SaveChanges();

            foreach (var comp in companies)
            {
                context.Companies.Add(comp);
            }
            context.SaveChanges();

        }

    }
}
