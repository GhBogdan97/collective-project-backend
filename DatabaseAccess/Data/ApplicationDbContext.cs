using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DatabaseAccess.Models;

namespace DatabaseAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Company>().ToTable("Companies");
            builder.Entity<Internship>().ToTable("Internships");
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Subscription>().ToTable("Subscriptions");
            builder.Entity<Application>().ToTable("Applications");
            builder.Entity<Rating>().ToTable("Ratings");
            builder.Entity<Post>().ToTable("Posts");

            //One to one Company-ApplicationUser
            builder.Entity<Company>(company => company.HasOne<ApplicationUser>()
                                             .WithOne()
                                             .HasForeignKey<Company>(c=>c.IdUser));

            //One to one Student-ApplicationUser
            builder.Entity<Student>(st => st.HasOne<ApplicationUser>()
                                             .WithOne()
                                             .HasForeignKey<Student>(s => s.IdUser));

            //many to many Company-Student

            builder.Entity<Subscription>()
                .HasKey(s => new { s.StudentId, s.CompanyId });

            builder.Entity<Subscription>()
                .HasOne(s => s.Student)
                .WithMany(st => st.Subscriptions)
                .HasForeignKey(st => st.StudentId);

            builder.Entity<Subscription>()
                .HasOne(s => s.Company)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(st => st.CompanyId);

            //many to many Internship-Student (Application)

            builder.Entity<Application>()
                .HasKey(a => new { a.InternshipId, a.StudentId });

            builder.Entity<Application>()
                .HasOne(a => a.Student)
                .WithMany(st => st.Applications)
                .HasForeignKey(a => a.StudentId);

            builder.Entity<Application>()
                .HasOne(a => a.Internship)
                .WithMany(i => i.Applications)
                .HasForeignKey(a => a.InternshipId);

            //one to many Company - Internship
            builder.Entity<Internship>()
                .HasOne(i => i.Company)
                .WithMany(c => c.Internships)
                .HasForeignKey(c => c.CompanyId);

            //many to many Internship-Student (Ratings)

            builder.Entity<Rating>()
                .HasKey(a => new { a.InternshipId, a.StudentId });

            builder.Entity<Rating>()
                .HasOne(r => r.Student)
                .WithMany(st => st.Ratings)
                .HasForeignKey(r => r.StudentId);

            builder.Entity<Rating>()
                .HasOne(r => r.Internship)
                .WithMany(i => i.Ratings)
                .HasForeignKey(r => r.InternshipId);

            //one to many Internship - Post
            builder.Entity<Post>()
                .HasOne(p => p.Internship)
                .WithMany(i=>i.Posts)
                .HasForeignKey(p => p.InternshipId);

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Post> Posts { get; set; }

    }
}
