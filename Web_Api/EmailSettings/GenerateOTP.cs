using System.Net;
using System.Net.Mail;

namespace Web_Api.EmailSettings
{
    public class GenerateOTP
    {

        // 1- Generating OTP Code
        public static string GenerateOTPCode()
        {
            // Generate a random 6-digit OTP
            Random random = new Random();
            var otp = random.Next(100000, 999999);
            return otp.ToString();
        }



        //2-  Sending OTP Code via Email
        public static async Task SendingOTPMessage(string ToEmail,string OTP)
        {
            // A) validate  OTP code

            if (string.IsNullOrEmpty(OTP) || OTP.Length != 6 || !int.TryParse(OTP, out _))
            {
                throw new ArgumentException("OTP must be a 6-digit number.");
            }
            // B) Validate email 
            if (string.IsNullOrEmpty(ToEmail))
            {
                throw new ArgumentException("Email  cannot be null or empty.");
            }
            #region Comments
            // Here you would implement the logic to send the OTP via email.
            // This could involve using an SMTP client or an email service provider's API.
            // port for Gmail 587 
            #endregion
            var EmailMessage = new MailMessage();
            // C) Set up the email message
            EmailMessage.From = new MailAddress("mmmelkady23@gmail.com");
            EmailMessage.To.Add(ToEmail);
            EmailMessage.Subject = "OTP Code for Verification";
            EmailMessage.Body = $"Your OTP Code is {OTP}. Code Is Valid For Five Minutes";

            //we using SmtpClient to send the email [Word Using Cause it's Un mange resource ]
            // D) Configure the SMTP client
            using var SMTP = new SmtpClient(host: "smtp.gmail.com", port: 587);
            SMTP.EnableSsl = true;
            SMTP.Credentials = new NetworkCredential(userName: "mmmelkady23@gmail.com", "Password");
            // E) Send the email
            try
            {
                await SMTP.SendMailAsync(EmailMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send OTP email.", ex);
            }
            finally
            {
                EmailMessage.Dispose(); // Dispose of the email message to free resources
            }
        }
    }
}
    