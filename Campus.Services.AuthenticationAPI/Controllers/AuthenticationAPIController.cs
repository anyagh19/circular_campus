using Campus.Services.AuthenticationAPI.Model.Dtos;
using Campus.Services.AuthenticationAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Campus.Services.AuthenticationAPI.Controllers
{
    [Route("api/AuthApi")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationAPIController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("CreateCampus")]
        public async Task<IActionResult> CreateCampus([FromBody] CampusDto request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                // Assuming you have an instance of IAuthenticationService injected into the controller
                var result = await _authenticationService.CreateCampus(request);
                if (result)
                {
                    return Ok("Campus created successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create campus.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (request == null)
            {
                return BadRequest("Data cannot be null");
            }
            var result = await _authenticationService.Login(request);
            if (result == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            return Ok(result);
        }
    }
}
