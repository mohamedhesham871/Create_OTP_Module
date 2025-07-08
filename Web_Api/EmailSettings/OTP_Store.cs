using Microsoft.AspNetCore.Components;
using StackExchange.Redis;

namespace Web_Api.EmailSettings
{
    public  class OTP_Store(IConnectionMultiplexer connection)
    {
        private readonly IDatabase connection = connection.GetDatabase();

        public   void StoreOTP(string email, string otp)
        {
            // Store the OTP with an expiration time of 5 minutes

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                throw new ArgumentException("Email and OTP cannot be null or empty.");
            }
            if (otp.Length != 6 || !int.TryParse(otp, out _))
            {
                throw new ArgumentException("OTP must be a 6-digit number.");
            }
            // Store OTP in a Redis with expiration time
            connection.StringSet(email, otp, TimeSpan.FromMinutes(5));

        }


        public  bool VerifyOTP(string email, string inputOtp)
        {
            // Validate input
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(inputOtp))
            {
                throw new ArgumentException("Email and OTP cannot be null or empty.");
            }
            if (inputOtp.Length != 6 || !int.TryParse(inputOtp, out _))
            {
                throw new ArgumentException("OTP must be a 6-digit number.");
            }
            // Retrieve the stored OTP from Redis
            var result = connection.StringGet(email);
            if (result!=inputOtp)
            {
                // OTP not found or expired
                return false;
            }
            return true;
        }
    }
}
