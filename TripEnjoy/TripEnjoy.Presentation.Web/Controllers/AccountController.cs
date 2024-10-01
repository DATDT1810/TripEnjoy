using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AccountDTO accountDTO)
        {
            if (accountDTO.email != null && accountDTO.password != null)
            {
                var result = await accountRepository.Login(accountDTO);
                if (result != null)
                {
                var tokens = new
                {
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken
                };
                    return Ok(tokens); // trả về chuỗi token
                }
            }
            return Unauthorized("Invalid credentials");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(AccountDTO accountDTO)
        {
            if (accountDTO.email != null && accountDTO.password != null)
            {     
                var result = await accountRepository.Register(accountDTO);
                if (result != null)
                {
                    return Ok(result);
                    // trả về thông tin user
                }
            }
            return StatusCode(500, "Create failed");
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
          if(refreshToken != null)
            {
                var newAccessToken = await accountRepository.RefreshToken(refreshToken);
                if(newAccessToken != null)
                {
                    return Ok(newAccessToken);
                }
            }
                return Unauthorized("Invalid credentials");
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (user != null)
            {
               var result = await accountRepository.Logout(user);
                if (result)
                {
                    return StatusCode(200);
                }          
            }
            return BadRequest();
        }

    }
}
