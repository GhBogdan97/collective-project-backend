using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
	{
		public EmailSettings _emailSettings { get; }

		public EmailSender(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}

        public Task SendEmailAsync(string email, string subject, string message)
		{
			Execute(email, subject, message).Wait();
			return Task.FromResult(0);
		}

		public async Task Execute(string email, string subject, string message)
		{
			try
			{
				string toEmail = string.IsNullOrEmpty(email)
								 ? _emailSettings.ToEmail
								 : email;
				MailMessage mail = new MailMessage()
				{
					From = new MailAddress(_emailSettings.UsernameEmail, "Inter Link")
				};
				mail.To.Add(new MailAddress(toEmail));
				mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

				mail.Subject = "Inter Link noutati - " + subject;
				mail.Body = message;
				mail.IsBodyHtml = true;
				mail.Priority = MailPriority.High;

				using (SmtpClient smtp = new SmtpClient(_emailSettings.SecondayDomain, _emailSettings.SecondaryPort))
				{
					smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
					smtp.EnableSsl = true;
					await smtp.SendMailAsync(mail);
				}
			}
			catch (Exception exception)
			{
				//do something here
			}
		}

	}
}
