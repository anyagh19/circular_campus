namespace Campus.Services.AuthenticationAPI.Model.Dtos
{
    public class CampusDto
    {
        public string CampusName { get; set; }
        public string CampusLocation { get; set; }
        public string CampusEmail { get; set; }
        public string CampusPhone { get; set; }
        public string CampusCode { get; set; }
        public string CampusPassword { get; set; }
    }

    public class LoginDto
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
