namespace BinghattiOtpVerificationAssessment.Models
{
    public class OtpModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string PhNo { get; set; }
        public string Otp { get; set; }
        public string EnteredOtp { get; set; }
        public bool IsVerified { get; set; }
    }
}
