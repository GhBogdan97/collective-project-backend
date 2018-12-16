using API.Mappers;
using API.ViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("internships")]
    [ApiController]
    //[Authorize]
    public class InternshipController : ControllerBase
    {

        private readonly InternshipService _internshipService;
        private readonly CompanyService _companyService;
        private readonly PostService _postService;
        private readonly StudentService _studentService;
        private readonly ApplicationService _applicationService;

        public InternshipController(StudentService studentService,
            InternshipService internshipService,
            CompanyService companyService,
            PostService postService,
            ApplicationService applicationService)
        {
            _internshipService = internshipService;
            _companyService = companyService;
            _postService = postService;
            _studentService = studentService;
            _applicationService = applicationService;
        }

        [HttpGet]
        [Route("{id}/management")]
        //[Authorize(Roles = "Company")]
        public ActionResult<List<ApplicationForManagementViewModel>> GetStudentManagementDetails(int id)
        {
            var applications = _applicationService.GetApplicationsForInternship(id);
            var applicationManagement = new List<ApplicationForManagementViewModel>();
            foreach (var app in applications)
            {
                var appManagement = ApplicationMapper.ToApplicationManagement(app);
                applicationManagement.Add(appManagement);
            }

            var obj = new ApplicationsListObject() { Applications = applicationManagement };
            return Ok(JsonConvert.SerializeObject(obj));
        }

        [HttpPost]
        [Route("{id}/students/select")]
        public async Task<ActionResult<ApplicationForManagementViewModel>> SelectStudentForInternshipAsync(int id, [FromBody] ApplicationForManagementViewModel applicationViewModel)
        {
            if (!_applicationService.ExistsApplication(applicationViewModel.Id, id))
            {
                return BadRequest($"Nu s-a gasit inregistrarea studentului {applicationViewModel.Fullname} la acest internship");
            }

            var student = _studentService.GetStudentById(applicationViewModel.Id);
            var internsip = _internshipService.GetInternshipById(id);
            await _applicationService.SelectStudentForInternshipAsync(student, internsip);
            applicationViewModel.Status = "CONTACTAT";
            return Ok(JsonConvert.SerializeObject(applicationViewModel));
        }

        [HttpPost]
        [Route("{id}/students/aprove")]
        public async Task<ActionResult<ApplicationForManagementViewModel>> ApproveStudentForInternshipAsync(int id, [FromBody] ApplicationForManagementViewModel applicationViewModel)
        {
            if (!_applicationService.ExistsApplication(applicationViewModel.Id, id))
            {
                return BadRequest($"Nu s-a gasit inregistrarea studentului {applicationViewModel.Fullname} la acest internship");
            }

            var student = _studentService.GetStudentById(applicationViewModel.Id);
            var internsip = _internshipService.GetInternshipById(id);
            await _applicationService.ApproveStudentForInternshipAsync(student, internsip);
            internsip.OccupiedPlaces++;
            _internshipService.UpdateInternship(internsip, id);
            applicationViewModel.Status = "APROBAT";
            return Ok(JsonConvert.SerializeObject(applicationViewModel));
        }

        [HttpPost]
        [Route("{id}/students/reject")]
        public async Task<ActionResult<ApplicationForManagementViewModel>> RejectStudentForInternshipAsync(int id, [FromBody] ApplicationForManagementViewModel applicationViewModel)
        {
            if (!_applicationService.ExistsApplication(applicationViewModel.Id, id))
            {
                return BadRequest($"Nu s-a gasit inregistrarea studentului {applicationViewModel.Fullname} la acest internship");
            }

            var student = _studentService.GetStudentById(applicationViewModel.Id);
            var internsip = _internshipService.GetInternshipById(id);
            await _applicationService.RejectStudentForInternshipAsync(student, internsip);
            applicationViewModel.Status = "RESPINS";
            return Ok(JsonConvert.SerializeObject(applicationViewModel));
        }

        [HttpGet]
        [Route("student")]
        [Authorize(Roles = "Student")]
        public ActionResult<List<InternshipMainAttributesViewModel>> GetInternshipsForStudent()
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if (claim != null)
            {
                var userId = claim.Value;
                try
                {
                    var studentId = _studentService.GetStudentIdForUser(userId);
                    var internshipsDb = _internshipService.GetInternshipsForStudent(studentId);
                    var viewModels = new List<InternshipMainAttributesViewModel>();
                    foreach (var internship in internshipsDb)
                    {
                        var viewModel = InternshipMapper.ToViewModel(internship);
                        viewModels.Add(viewModel);
                    }
                    return Ok(viewModels);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Studentul nu a fost recunoscut");
        }

        [HttpGet]
        [Route("{id:int}/posts")]
        public ActionResult<PostObjectViewModels> GetPostsForInternship(int id)
        {
            try
            {
                var posts = _postService.GetPostsForInternship(id);
                var postViewModels = new List<PostViewModel>();
                foreach (var post in posts)
                {
                    var postModel = PostMapper.ToPostViewModel(post);
                    postViewModels.Add(postModel);
                }
                var postObjects = new PostObjectViewModels()
                {
                    Posts = postViewModels
                };
                return Ok(postObjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Company")]
        public ActionResult<List<InternshipMainAttributesViewModel>> GetAllInternships()
        {
            var userId = User.GetUserId();
            if (userId != string.Empty)
            {
                try
                {
                    var companyId = _companyService.GetCompanyIdForUser(userId);
                    var internshipsDB = _internshipService.GetInternshipsForCompany(companyId);
                    var viewModels = new List<InternshipMainAttributesViewModel>();
                    foreach (var internship in internshipsDB)
                    {
                        var viewModel = Mappers.InternshipMapper.ToViewModel(internship);
                        viewModels.Add(viewModel);
                    }
                    return Ok(viewModels);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return BadRequest("Compania nu a fost recunoscuta");
        }


        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Company")]
        public ActionResult<InternshipMainAttributesViewModel> AddInternship(InternshipAddViewModel internship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userID = User.GetUserId();
                if (userID == string.Empty)
                {
                    return BadRequest("Compania nu a fost recunoscuta");
                }
                var companyID = _companyService.GetCompanyIdForUser(userID);
                var addedInternship = _internshipService.AddInternship(InternshipAddMapper.ToInternship(internship, companyID));


                return Ok(InternshipMapper.ToViewModel(addedInternship));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [Route("{id:int}")]
        [HttpPut]
        [Authorize(Roles = "Company")]
        public IActionResult UpdateInternship([FromBody] InternshipMainAttributesViewModel internshipView, int id)
        {
            try
            {
                var internship = InternshipMapper.ToActualInternshipObject(internshipView);
                _internshipService.UpdateInternship(internship, id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [Route("{id:int}/posts")]
        [HttpPost]
        //[Authorize(Roles = "Company")]
        public IActionResult SavePost([FromBody] PostViewModel postView, int id)
        {
            try
            {
                var post = PostMapper.ToActualPostObject(postView, id);
                var addedPost = _postService.SavePost(post);
                postView.Id = addedPost.Id;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(postView);
        }

        [HttpGet]
        [Route("{id}/testimonials")]
        public ActionResult<List<TestimonialViewModel>> GetTestimonialForInternship(int id)
        {
            var ratings = _internshipService.GetInternshipRatings(id);
            var testimonials = new List<TestimonialViewModel>();
            foreach (var rating in ratings)
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
            return Ok(new { testimonials });
        }

        [HttpGet]
        [Route("details/{id}")]
        [Authorize(Roles = "Company,Student")]
        public ActionResult<InternshipDetailsRatingViewModel> GetInternshipById(int id)
        {
            try
            {
                var internship = _internshipService.GetInternshipDetails(id);
                var ratingDTO = _internshipService.GetInternshipRatingsAverege(id);
                var internshipDetailsRating = InternshipDetailsRatingMapper.ToInternshipDetailsRating(internship, ratingDTO);
                return Ok(internshipDetailsRating);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("availability/{id}")]
        public IActionResult GetInternshipAvailability(int id)
        {
            var internship = _internshipService.GetInternshipById(id);
            var internshipViewModel = InternshipMapper.ToInternshipManagementViewModel(internship);
            return Ok(JsonConvert.SerializeObject(internshipViewModel));
        }


	}
}