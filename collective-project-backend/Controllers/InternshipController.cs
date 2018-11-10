using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Services;


namespace API.Controllers
{
    [Route("internships")]
    [ApiController]
    public class InternshipController : ControllerBase
    {

        private readonly InternshipService _internshipService;

        public InternshipController(InternshipService internshipService)
        {
            _internshipService = internshipService;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddInternship(Internship internship)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _internshipService.AddInternship(internship);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
    

        [HttpPut]
        public IActionResult updateInternship(Internship internship)
        {
            try
            {
                _internshipService.UpdateInternship(internship);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpGet]
        [Route("{id}/testimonials")]
        public ActionResult<TestimonialViewModel> GetTestimonialForInternship(int id)
        {
            var ratings = _internshipService.GetInternshipRatings(id);
            var testimonials = new List<TestimonialViewModel>();
            foreach(var rating in ratings)
            {
                var testimonial = new TestimonialViewModel()
                {
                    Firstname = rating.Student.Firstname,
                    Lastname = rating.Student.Lastname,
                    Testimonial = rating.Testimonial,
                    Date = rating.Date.Date.ToShortDateString()
                };
                testimonials.Add(testimonial);
            }
            return Ok(testimonials);
        }
    }
}