using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Sehatak.Application.DTOs.PatienRegisterDto;
using Sehatak.Application.Interfaces.AuthPatient;

namespace Sehatak.API.Controllers.Patient
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
            
        }
        
        [HttpPost("RegisterPatient")]
        public async Task<IActionResult> RegisterPatient ([FromForm] RegisterRequestDto registerRequestDto)
        {
            var result = await authService.RegisterAsync(registerRequestDto);
            return Ok(result);
        }
        
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyOtpRequestDto request)
        {
            var result = await authService.VerifyOtpAsync(request);

            if (result == null)
                throw new ArgumentException("الكود غير صحيح أو منتهي الصلاحية.");

            return Ok(result);
        }
    }
}
