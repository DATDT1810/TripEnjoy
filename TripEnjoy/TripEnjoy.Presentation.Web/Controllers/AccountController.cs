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
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {           
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AccountDTO accountDTO)
        {
            if (accountDTO.email != null && accountDTO.password != null)
            {
                var result = await accountService.Login(accountDTO);
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
                var result = await accountService.Register(accountDTO);
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
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshDTO tokenRefreshDTO)
        {
          if(tokenRefreshDTO.refreshToken != null)
            {
                var tokens = await accountService.RefreshToken(tokenRefreshDTO.refreshToken);
                if(tokens != null)
                {
                    return Ok(tokens);
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
               var result = await accountService.Logout(user);
                if (result)
                {
                    return StatusCode(200);
                }          
            }
            return BadRequest();
        }
        
        [HttpPost]
        [Route("LoginGoogle")]
        public async Task<IActionResult> LoginGoogle([FromBody] EmailDTO emailDTO)
        {
            if(emailDTO.email != null)
            {
                var result = await accountService.LoginGoogle(emailDTO.email);
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

    }
}
