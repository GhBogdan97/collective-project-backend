using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class RegisterStudentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string University { get; set; }
        public string Specialization { get; set; }
        public string College { get; set; }
        public int Year { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public IFormFile Cv { get; set; }
    }
}
