using System;
using System.Collections.Generic;
using System.Linq;
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

		public IList<Post> GetPostsForInternship(int id)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.InternshipRepository.GetById(id) == null)
				{
					throw new Exception("Internship inexistent");
				}
				var postsForInternship = new List<Post>();
				IList<Post> posts = uow.PostRepository.GetAll();
				foreach (Post post in posts)
				{
					if (post.InternshipId == id)
					{
						postsForInternship.Add(post);
					}
				}
				return postsForInternship.OrderByDescending(x => x.Date).ToList();
			}
		}
	}
}
