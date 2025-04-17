using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BinghattiOtpVerificationAssessment.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class OtpApiController : ControllerBase
    {
        [HttpPost]
        public string GenerateOtp(string fullName, string emailId)
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
            bool isSentEmail = SendOtpEmail(fullName, emailId, otp);

            return otp;
        }
        public bool SendOtpEmail(string fullName, string emailId, string otp)
        {
            bool isEmailSent=false;

            try
            {
                SmtpClient mySmtpClient = new SmtpClient("smtp.gmail.com", 587);
                mySmtpClient.EnableSsl = true;

                mySmtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new
                   System.Net.NetworkCredential("testing2020.testing2020@gmail.com", "dffqlqzwmfxlnmib");
                mySmtpClient.Credentials = basicAuthenticationInfo;

                MailAddress from = new MailAddress("testing2020.testing2020@gmail.com", "Sruthy");
                MailAddress to = new MailAddress(emailId, fullName);
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
                              

                myMail.Subject = "Otp Verification - Testing";
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                myMail.Body = "<b>Dear "+fullName+",<br>The generated OTP is : <b>" + otp + "</b>.<br>Regards";
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
                isEmailSent = true;
            }

            catch (SmtpException ex)
            {
                //throw new ApplicationException
                //  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            return isEmailSent;
        }

      
    }
}
