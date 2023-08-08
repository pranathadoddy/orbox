namespace Orderbox.ServiceContract.Authentication.Request
{
    public class ResetPasswordRequest
    {
        public string EmailAddress { get; set; }

        public string PasswordResetToken { get; set; }

        public string NewPassword { get; set; }
    }
}
