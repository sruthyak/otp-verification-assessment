﻿namespace BinghattiOtpVerificationAssessment.Models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderPassword { get; set; }
        public string Subject { get; set; }
        public string BodyTemplate { get; set; }
    }
}
