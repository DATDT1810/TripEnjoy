using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.EmailService;
using TripEnjoy.Application.Interface.ImageCloud;
using TripEnjoy.Infrastructure.Service;


namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IEmailService emailService;
        private readonly IImageService imageService;

        public AccountController(IAccountService accountService, IEmailService emailService, IImageService imageService )
        {
            this.accountService = accountService;
            this.emailService = emailService;
            this.imageService = imageService;
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
            if (tokenRefreshDTO.refreshToken != null)
            {
                var tokens = await accountService.RefreshToken(tokenRefreshDTO.refreshToken);
                if (tokens != null)
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
            if (emailDTO.email != null)
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

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordRequest passwordRequest)
        {
            if (passwordRequest != null)
            {
                
                var code = HttpContext.Session.GetString("code")?.Trim();
             
                if(passwordRequest.code.Trim().Equals(code))
                {
                    
                    var result = await accountService.ResetPassword(passwordRequest.email,passwordRequest.password);
                    if (result)
                    {
                        HttpContext.Session.Remove("code");
                        return Ok();
                    }              
                }
               
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromBody] EmailDTO emailDTO)
        {
            if (emailDTO.email != null)
            {
                var result = await accountService.CheckEmail(emailDTO.email);
                if (result != null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("SendCode")]
        public async Task<IActionResult> SendCode(EmailDTO emailDTO)
        {
            if (emailDTO != null)
            {
                MailRequest mailRequest = new MailRequest();
                mailRequest.ToEmail = emailDTO.email;
                mailRequest.Subject = "Reset Password";

                // tạo mã code 6 số lưu vào session
                var code = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("code", code);

                string content = "This is the verify code to reset your password";
                mailRequest.Body = emailService.GetCodeHtmlContent(content, code); ;
                await emailService.SendEmailAsync(mailRequest);
                return Ok();
            }
            return BadRequest();
        }

    }
}
