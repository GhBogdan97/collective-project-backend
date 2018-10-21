using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace collective_project_backend.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
