using System.Net.Mail;
using BinghattiOtpVerificationAssessment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BinghattiOtpVerificationAssessment.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class OtpApiController : ControllerBase
    {
        private readonly EmailSettings _emailSettings;
        public OtpApiController(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        [HttpPost]
        public async Task<bool> GenerateOtp(string fullName, string emailId)
        {
            bool isSuccess = true;
            try
            {
                string[] arr = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                string otp = "";
                string temp;
                Random rand = new Random();
                for (int i = 0; i < 6; i++)
                {
                    temp = arr[rand.Next(0, arr.Length)];
                    otp += temp;
                }
                bool isSentEmail = await SendOtpEmail(fullName, emailId, otp);
                isSuccess &= isSentEmail;
                if (isSentEmail) {
                    HttpContext.Session.SetString(emailId, otp);
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        [HttpPost]
        public bool VerifyOtp(string emailId, string otp)
        {
            bool isSuccess = false;
            string? sessionOtp = HttpContext.Session.GetString(emailId);

            if (sessionOtp == otp) { 
                isSuccess = true; 
            }

            return isSuccess;
        }
        public async Task<bool> SendOtpEmail(string fullName, string emailId, string otp)
        {
            bool isEmailSent=false;
            
            SmtpClient mySmtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port);
            mySmtpClient.EnableSsl = true;

            mySmtpClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicAuthenticationInfo = new
                System.Net.NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            mySmtpClient.Credentials = basicAuthenticationInfo;

            MailAddress from = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
            MailAddress to = new MailAddress(emailId, fullName);
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
                              

            myMail.Subject = _emailSettings.Subject;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            myMail.Body = string.Format(_emailSettings.BodyTemplate, fullName, otp);
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            myMail.IsBodyHtml = true;

            await mySmtpClient.SendMailAsync(myMail);
            isEmailSent = true;            

            return isEmailSent;
        }

      
    }
}
