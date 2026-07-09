using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
        [EnableRateLimiting("LoginPolicy")]
        [HttpPost("RegisterPatient{centerId}")]
        public async Task<IActionResult> RegisterPatient (int centerId,[FromForm] RegisterRequestDto registerRequestDto)
        {
            var result = await authService.RegisterAsync(centerId,registerRequestDto);
            return Ok(result);
        }
        
        [HttpPost("verify-code{centerId}")]
        public async Task<IActionResult> VerifyCode(int centerId,[FromBody] VerifyOtpRequestDto request)
        {
            var result = await authService.VerifyOtpAsync(centerId,request);

            if (result == null)
                throw new ArgumentException("الكود غير صحيح أو منتهي الصلاحية.");

            return Ok(result);
        }
    }
}
