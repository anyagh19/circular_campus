using Campus.Services.AuthenticationAPI.Model.Dtos;

namespace Campus.Services.AuthenticationAPI.Services
{
    public interface IAuthenticationService
    {
        public Task<bool> CreateCampus(CampusDto request);
        public Task<LoginResponseDto> Login(LoginDto request);
    }
}
