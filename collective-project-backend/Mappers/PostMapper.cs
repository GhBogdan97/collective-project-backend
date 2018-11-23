using API.ViewModels;
using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
    public class PostMapper
    {
        public static Post ToActualPostObject(PostViewModel postView, int id)
        {
            return new Post()
            {
                InternshipId = id,
                Date = DateTime.Parse(postView.Date),
                Title = postView.Title,
                Last = postView.Last,
                Image = postView.Image,
                Text=postView.Text
            };
        }

        public static PostViewModel ToPostViewModel(Post post)
        {
            return new PostViewModel()
            {
                Date = post.Date.ToString(),
                Title = post.Title,
                Last = post.Last,
                Image = post.Image,
                Text=post.Text
            };
        }
    }
}