using DatabaseAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            await userManager.CreateAsync(new ApplicationUser { Email = "bogdan_2101@yahoo.com", UserName = "bogdan_2101@yahoo.com" }, "Password123.");
            await userManager.CreateAsync(new ApplicationUser { Email = "simonaburlacu01@gmail.com", UserName = "simonaburlacu01@gmail.com" }, "Password123.");
            await userManager.CreateAsync(new ApplicationUser { Email = "testStudent1@email.com", UserName = "testStudent1@email.com" }, "Password123.");
            await userManager.CreateAsync(new ApplicationUser { Email = "testCompanyUser1@email.com", UserName = "testCompanyUser1@email.com" }, "Password123.");
            await userManager.CreateAsync(new ApplicationUser { Email = "testCompanyUser2@email.com", UserName = "testCompanyUser2@email.com" }, "Password123.");

            await roleManager.CreateAsync(new IdentityRole { Name = "Student" });
            await roleManager.CreateAsync(new IdentityRole { Name = "Company" });

            var student1 = await userManager.FindByEmailAsync("simonaburlacu01@gmail.com");
            var student2 = await userManager.FindByEmailAsync("testStudent1@email.com");
            var student3 = await userManager.FindByEmailAsync("bogdan_2101@yahoo.com");
            var company1 = await userManager.FindByEmailAsync("testCompanyUser1@email.com");
            var company2 = await userManager.FindByEmailAsync("testCompanyUser2@email.com");

            var roleStudent = await roleManager.FindByNameAsync("Student");
            var roleCompany = await roleManager.FindByNameAsync("Company");

            #region Set Roles
            await userManager.AddToRoleAsync(student1, roleStudent.Name);
            await userManager.AddToRoleAsync(student2, roleStudent.Name);
            await userManager.AddToRoleAsync(student3, roleStudent.Name);
            await userManager.AddToRoleAsync(company1, roleCompany.Name);
            await userManager.AddToRoleAsync(company2, roleCompany.Name);
            #endregion

            #region Students
            List<Student> students = new List<Student> {
                new Student()
                {
                    Firstname = "Simona",
                    Lastname = "Burlacu",
                    University = "UBB",
                    College = "Facultatea de Matematica-Informatica",
                    Specialization = "Informatica-Romana",
                    Cv = System.IO.File.ReadAllBytes("E:\\Downloads\\cvExample.pdf"),
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
                },
                new Student()
                {
                    Firstname = "Bogdan",
                    Lastname = "Gheorghe",
                    University = "UBB",
                    College = "Facultatea de Matematica-Informatica",
                    Specialization = "Informatica-Romana",
                    Year = 3,
                    IdUser = student3.Id
                }
            };
            #endregion

            #region Companies
            List<Company> companies = new List<Company> {
                new Company
                {
                    Name = "Accesa", 
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
            #endregion

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

            #region Internships
            var accesa = context.Companies.Where(c => c.Name == "Accesa").FirstOrDefault();
            Internship internshipAccesa1 = new Internship()
            {
                Description = "Internship React",
                Places = 8,
                Topics = "React, JavaScript",
                Start = new DateTime(2018, 3, 3),
                End = new DateTime(2018, 5, 3),
                Name= "Best Internship EU",
                Weeks = 2,
                CompanyId=accesa.Id
            };

            Internship internshipAccesa2 = new Internship()
            {

                Description = "Internship Azure",
                Places = 8,
                Topics = "Azure functions, Serverless",
                Start = new DateTime(2017, 10, 3),
                End = new DateTime(2017, 11, 3),
                Name= "Internship Cloud",
                Weeks = 2,
                CompanyId=accesa.Id
            };

            Internship internshipAccesa3 = new Internship()
            {

                Description = "Internship Mobile",
                Places = 8,
                Topics = "React Native, Android",
                Start = new DateTime(2019,1, 1),
                End = new DateTime(2019, 5, 1),
                Name = "Internship Aplicatii Mobile",
                Weeks = 16,
                CompanyId = accesa.Id
            };

            Internship internshipAccesa4 = new Internship()
            {

                Description = "Internship Machine Learning",
                Places = 8,
                Topics = "AI, ML, Deep Learning",
                Start = new DateTime(2019, 1, 1),
                End = new DateTime(2019, 3, 1),
                Name = "Internship Inteligenta Artificiala",
                Weeks = 8,
                CompanyId = accesa.Id
            };

            context.Internships.Add(internshipAccesa1);
            context.Internships.Add(internshipAccesa2);
            context.Internships.Add(internshipAccesa3);
            context.Internships.Add(internshipAccesa4);
            context.SaveChanges();

            accesa.Internships.Add(internshipAccesa1);
            accesa.Internships.Add(internshipAccesa2);
            accesa.Internships.Add(internshipAccesa3);
            accesa.Internships.Add(internshipAccesa4);
            context.SaveChanges();
            #endregion

            var simona = context.Students.Where(s => s.Firstname == "Simona").FirstOrDefault();
            var ionescu= context.Students.Where(s => s.Firstname == "Ionescu").FirstOrDefault();
            var bogdan = context.Students.Where(s => s.Firstname == "Bogdan").FirstOrDefault();

            #region Applications
            var application1 = new Application()
            {
                InternshipId=internshipAccesa1.Id,
                StudentId=simona.Id,
                Status=Enums.ApplicationStatus.APLICAT
            };
            context.Applications.Add(application1);
            context.SaveChanges();

            var application2 = new Application()
            {
                InternshipId = internshipAccesa2.Id,
                StudentId = simona.Id,
                Status = Enums.ApplicationStatus.CONTACTAT
            };
            context.Applications.Add(application2);
            context.SaveChanges();

            var application3 = new Application()
            {
                InternshipId = internshipAccesa1.Id,
                StudentId = ionescu.Id,
                Status = Enums.ApplicationStatus.EXAMINARE
            };
            context.Applications.Add(application3);
            context.SaveChanges();

            var application4 = new Application()
            {
                InternshipId = internshipAccesa1.Id,
                StudentId = bogdan.Id,
                Status = Enums.ApplicationStatus.CONTACTAT
            };
            context.Applications.Add(application4);
            context.SaveChanges();

            string imageFilePath1 = "/DatabaseAccess/Resources/internship2.jpg";
            string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            Post post1 = new Post()
            {
                Title = "Un nou internship",
                Date = new DateTime(2019, 1, 30),
                InternshipId = 1,
                Last = false,
                Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Image = File.ReadAllBytes(parentDirectory + imageFilePath1)
        };

            context.Posts.Add(post1);
            context.SaveChanges();


            string imageFilePath2 = "/DatabaseAccess/Resources/internship1.png";
            Post post2 = new Post()
            {
                Title = "Detalii despre selectie",
                Date = new DateTime(2019, 2, 10),
                InternshipId = 1,
                Last = false,
                Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Image = File.ReadAllBytes(parentDirectory + imageFilePath2)
            };

            context.Posts.Add(post2);
            context.SaveChanges();
            #endregion

            #region Ratings

            List<Rating> ratings = new List<Rating>
            {
                new Rating
                {
                    InternshipId = 1,
                    StudentId = 1,
                    RatingCompany = 4,
                    RatingInternship = 5,
                    RatingMentors = 5,
                    Date = new DateTime(2018,05,10),
                    Testimonial = "Cel mai fain internship din toate (1) pe care le-am avut!"
                },
                new Rating
                {
                    InternshipId = 1,
                    StudentId = 2,
                    RatingCompany = 3,
                    RatingInternship = 3,
                    RatingMentors = 3,
                    Date = new DateTime(2018,07,23),
                    Testimonial = "Meh."
                },
            };

            foreach (var rating in ratings)
            {
                context.Ratings.Add(rating);
            }
            context.SaveChanges();

            #endregion  
        }
    }

   
}
