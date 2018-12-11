﻿// <auto-generated />
using System;
using DatabaseAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabaseAccess.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181206193639_AddedApplicationStatus")]
    partial class AddedApplicationStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DatabaseAccess.Models.Application", b =>
                {
                    b.Property<int>("InternshipId");

                    b.Property<int>("StudentId");

                    b.Property<int>("Status");

                    b.HasKey("InternshipId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("DatabaseAccess.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("IdUser");

                    b.Property<byte[]>("Logo");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("IdUser")
                        .IsUnique()
                        .HasFilter("[IdUser] IS NOT NULL");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Internship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("End");

                    b.Property<int>("Places");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Topics");

                    b.Property<int>("Weeks");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Internships");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<byte[]>("Image");

                    b.Property<int>("InternshipId");

                    b.Property<bool>("Last");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("InternshipId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Rating", b =>
                {
                    b.Property<int>("InternshipId");

                    b.Property<int>("StudentId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("RatingCompany");

                    b.Property<int>("RatingInternship");

                    b.Property<int>("RatingMentors");

                    b.Property<string>("Testimonial");

                    b.HasKey("InternshipId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("College");

                    b.Property<byte[]>("Cv");

                    b.Property<string>("Firstname");

                    b.Property<string>("IdUser");

                    b.Property<string>("Lastname");

                    b.Property<string>("Specialization");

                    b.Property<string>("University");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("IdUser")
                        .IsUnique()
                        .HasFilter("[IdUser] IS NOT NULL");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Subscription", b =>
                {
                    b.Property<int>("StudentId");

                    b.Property<int>("CompanyId");

                    b.HasKey("StudentId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Application", b =>
                {
                    b.HasOne("DatabaseAccess.Models.Internship", "Internship")
                        .WithMany("Applications")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseAccess.Models.Student", "Student")
                        .WithMany("Applications")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseAccess.Models.Company", b =>
                {
                    b.HasOne("DatabaseAccess.Models.ApplicationUser")
                        .WithOne()
                        .HasForeignKey("DatabaseAccess.Models.Company", "IdUser");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Internship", b =>
                {
                    b.HasOne("DatabaseAccess.Models.Company", "Company")
                        .WithMany("Internships")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseAccess.Models.Post", b =>
                {
                    b.HasOne("DatabaseAccess.Models.Internship", "Internship")
                        .WithMany("Posts")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseAccess.Models.Rating", b =>
                {
                    b.HasOne("DatabaseAccess.Models.Internship", "Internship")
                        .WithMany("Ratings")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseAccess.Models.Student", "Student")
                        .WithMany("Ratings")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseAccess.Models.Student", b =>
                {
                    b.HasOne("DatabaseAccess.Models.ApplicationUser")
                        .WithOne()
                        .HasForeignKey("DatabaseAccess.Models.Student", "IdUser");
                });

            modelBuilder.Entity("DatabaseAccess.Models.Subscription", b =>
                {
                    b.HasOne("DatabaseAccess.Models.Company", "Company")
                        .WithMany("Subscriptions")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseAccess.Models.Student", "Student")
                        .WithMany("Subscriptions")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DatabaseAccess.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DatabaseAccess.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseAccess.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DatabaseAccess.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
