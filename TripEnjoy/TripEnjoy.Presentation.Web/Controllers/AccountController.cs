using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return StatusCode(500, "Invalid Account");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (email != null && password != null)
            {
                AccountDTO accountDTO = new AccountDTO
                {
                    email = email,
                    password = password
                };
                var result = await accountRepository.Register(accountDTO);
                if (result != null)
                {
                    return Ok(result);
                    // trả về thông tin user
                }
            }
            return StatusCode(500, "Create failed");
        }
    }
}
