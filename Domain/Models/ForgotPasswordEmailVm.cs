namespace Domain.Models
{
    public class ForgotPasswordEmailVm
    {
        public string Link { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}