﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class PostService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PostService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public Post SavePost(Post post)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var internship = uow.InternshipRepository.getDbSet()
                    .Where(i => i.Id == post.InternshipId)
                    .Include(i => i.Applications)
                    .FirstOrDefault();

                if(internship == null)
                {
                    throw new Exception("Internship inexistent");
                }
                uow.PostRepository.AddEntity(post);
                uow.Save();

                var usersEmails = new List<string>();

                foreach (var application in internship.Applications)
                {
                    var student = uow.StudentRepository.GetById(application.StudentId);
                    var user = _userManager.FindByIdAsync(student.IdUser).Result;

                    usersEmails.Add(user.Email);

                }

                Task emailSenderTask = new Task(new Action(() =>
                {
                    var emailSender = new EmailSender();
                    emailSender.SendEmailsAsync(usersEmails, "Internship news!", "New information regarding the " + internship.Description + " internship has been posted! Check our application for more information! ;)");
                }));
                emailSenderTask.Start();

                return post;
            }
        }

        public List<Post> GetPostsForInternship(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var internship = uow.InternshipRepository.getDbSet()
                    .Where(i => i.Id == id)
                    .Include(i => i.Posts)
                    .FirstOrDefault();

                if (internship == null)
                {
                    throw new Exception("Internship inexistent");
                }
                return internship.Posts.OrderByDescending(p=>p.Date).ToList();
            }
        }
    }
}
