using Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths;
using Co_OwnerManagementSystem.AuthenticationBusiness.Services.Auth;
using Co_OwnerManagementSystem.AuthenticationBusiness.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Co_OwnerManagementSystem.AuthenticationApi.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService  authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel requestModel)
        {
            var response = await _authService.Login(requestModel);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequestModel model)
        {
            var response = await _authService.Register(model);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            int userId = ClaimsPrincipalHelper.GetUserId(HttpContext.User);
            var profile = await _authService.GetMyProfile(userId);
            return Ok(profile);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            await _tokenService.InvalidateToken(refreshToken);
            return Ok();
        }
        
        [Authorize]
        [HttpPatch("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody]UpdateProfileModel model)
        {
            
            int userId = ClaimsPrincipalHelper.GetUserId(HttpContext.User);
            var result = await _authService.UpdateProfile(userId, model);
            return Ok(result);
        }
    }
}
