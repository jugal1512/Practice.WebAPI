namespace Practice.WebAPI.Models
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public bool TwoFactorAuthenticate { get; set; }
    }
}
