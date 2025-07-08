using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Web_Api.EmailSettings;

namespace Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OTPmessageContoller(IConnectionMultiplexer connection) : ControllerBase
    {
        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTP(string email)
        {
            if (email == null || email.Trim() == "")
            {
                return BadRequest("Email cannot be null or empty.");
            }
            var OTP = GenerateOTP.GenerateOTPCode();
            // Store the OTP in Redis with a 5-minute expiration
          
            var OTP_Store = new OTP_Store(connection);
            OTP_Store.StoreOTP(email, OTP);

            await GenerateOTP.SendingOTPMessage(email, OTP);

            return Ok();
        }
        [HttpPost("VerifyOTP")]
        public IActionResult VerifyOTP(string email, string otp)
        {
           var OTP_Store = new OTP_Store(connection);

            var isValid = OTP_Store.VerifyOTP(email, otp);
            if (isValid)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Invalid or expired OTP.");
            }
        }


    }
}
