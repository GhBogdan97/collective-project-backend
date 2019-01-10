using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class UploadCvViewModel
    {
        public IFormFile Cv { get; set; }
    }
}
