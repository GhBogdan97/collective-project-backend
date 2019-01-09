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
    [ApiController]
    [Authorize]
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
        [Route("internships/{id}/management")]
        [Authorize(Roles = "Company")]
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

        [HttpGet]
        [Route("student-internships")]
        public ActionResult<List<InternshipForManagementViewModel>> GetInternshipsForStudentManagement()
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if (claim != null)
            {
                var userId = claim.Value;
                try
                {
                    var studentId = _studentService.GetStudentIdForUser(userId);
                    var internships = _internshipService.GetInternshipsForStudent(studentId);
                    var internshipManagement = new List<InternshipForManagementViewModel>();
                    foreach (var intern in internships)
                    {
                        var companyName = _internshipService.GetCompanyNameForInternship(intern);
                        var status = _internshipService.GetStatusForStudentInternship(intern, studentId);
                        var internManagement = InternshipMapper.ToInternshipManagement(intern, status, companyName);
                        internshipManagement.Add(internManagement);
                    }

                    var obj = new InternshipsListObject() { Internships = internshipManagement };
                    return Ok(JsonConvert.SerializeObject(obj));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Studentul nu a fost recunoscut");
        }

        [HttpPost]
        [Route("internships/{id}/students/select")]
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
        [Route("internships/{id}/students/aprove")]
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
        [Route("internships/{id}/students/reject")]
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

        [HttpPut]
        [Route("internships/students/refuse")]
        public ActionResult<InternshipForManagementViewModel> RejectInternshipForStudentAsync([FromBody] InternshipForManagementViewModel internshipViewModel)
        {
            
            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if (claim != null)
            {
                var userId = claim.Value;
                try
                {
                    var studentId = _studentService.GetStudentIdForUser(userId);
                    if (!_applicationService.ExistsApplication(studentId, internshipViewModel.Id))
                    {
                        return BadRequest("Nu s-a gasit inregistrarea studentului pentru acest internship");
                    }

                    _applicationService.RejectInternshipForStudent(studentId, internshipViewModel.Id);
                    internshipViewModel.Status = "RESPINS";
                    return Ok(JsonConvert.SerializeObject(internshipViewModel));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Studentul nu a fost recunoscut");
        }

        [HttpPut]
        [Route("internships/students/confirm-exam-attendance")]
        public ActionResult<InternshipForManagementViewModel> ConfirmInternshipExamAttendanceForStudentAsync([FromBody] InternshipForManagementViewModel internshipViewModel)
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if (claim != null)
            {
                var userId = claim.Value;
                try
                {
                    var studentId = _studentService.GetStudentIdForUser(userId);
                    if (!_applicationService.ExistsApplication(studentId, internshipViewModel.Id))
                    {
                        return BadRequest("Nu s-a gasit inregistrarea studentului pentru acest internship");
                    }

                    _applicationService.ConfirmInternshipExamAttendanceForStudent(studentId, internshipViewModel.Id);
                    internshipViewModel.Status = "EXAMINARE";
                    return Ok(JsonConvert.SerializeObject(internshipViewModel));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Studentul nu a fost recunoscut");
        }

        [HttpPut]
        [Route("internships/students/confirm-participation")]
        public ActionResult<InternshipForManagementViewModel> ConfirmInternshipParticipationForStudentAsync([FromBody] InternshipForManagementViewModel internshipViewModel)
        {
            
            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if (claim != null)
            {
                var userId = claim.Value;
                try
                {
                    var studentId = _studentService.GetStudentIdForUser(userId);
                    if (!_applicationService.ExistsApplication(studentId, internshipViewModel.Id))
                    {
                        return BadRequest("Nu s-a gasit inregistrarea studentului pentru acest internship");
                    }

                    _applicationService.ConfirmInternshipParticipationForStudent(studentId, internshipViewModel.Id);
                    internshipViewModel.Status = "ADMIS";
                    return Ok(JsonConvert.SerializeObject(internshipViewModel));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Studentul nu a fost recunoscut");
        }

        [HttpGet]
        [Route("internships/student")]
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
        [Route("internships/{id}/posts")]
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
        [Route("internships")]
        [Authorize(Roles="Company")]
        public ActionResult<List<InternshipMainAttributesViewModel>> GetAllInternships()
        {
            var userId = User.GetUserId();
            if (userId != string.Empty)
            {
                try
                {
                    var companyId = _companyService.GetCompanyIdForUser(userId);
                    var internshipsDB = _internshipService.GetInternshipsForCompany(companyId);
                    var closedInternships = internshipsDB.Where(i => i.End <= DateTime.Now)
                                            .OrderByDescending(i=>i.End);
                    var activeInternships = internshipsDB.Where(i => i.End > DateTime.Now)
                                            .OrderBy(i=>i.End);
                    var sortedInternships = activeInternships.Union(closedInternships);
                    var viewModels = new List<InternshipMainAttributesViewModel>();
                    foreach (var internship in sortedInternships)
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
        [Route("internships/add")]
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

        [HttpPut]
        [Route("internships/{id}")]    
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

        [HttpPost]
        [Route("internships/{id}/posts")]
        [Authorize(Roles = "Company")]
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
        [Route("internships/{id}/testimonials")]
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
        [Route("internships/details/{id}")]
        [Authorize(Roles="Company,Student")]
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
        [Route("internships/availability/{id}")]
        public IActionResult GetInternshipAvailability(int id)
        {
            var internship = _internshipService.GetInternshipById(id);
            var internshipViewModel = InternshipMapper.ToInternshipManagementViewModel(internship);
            return Ok(JsonConvert.SerializeObject(internshipViewModel));
        }


	}
}