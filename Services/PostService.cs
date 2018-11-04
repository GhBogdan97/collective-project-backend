using System;
using System.Collections.Generic;
using System.Text;
using DatabaseAccess.Models;
using DatabaseAccess.UOW;

namespace Services
{
    public class PostService
    {
        public void SavePost(Post post)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if(uow.InternshipRepository.GetById(post.InternshipId) == null)
                {
                    throw new Exception("Internship inexistent");
                }
                uow.PostRepository.AddEntity(post);
                uow.Save();
            }
        }
    }
}
