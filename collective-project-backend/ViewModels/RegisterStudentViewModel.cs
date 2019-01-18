using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class RegisterStudentViewModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string University { get; set; }
        public string Specialization { get; set; }
        public string College { get; set; }
        public int Year { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IFormFile Cv { get; set; }
    }
}
