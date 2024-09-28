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
        public async Task<IActionResult> Login(string email, string password)
        {
            if (email != null && password != null)
            {
                AccountDTO accountDTO = new AccountDTO
                {
                    email = email,
                    password = password
                };
                var result = await accountRepository.Login(accountDTO);
                if (result != null)
                {
                    return Ok(result);
                    // trả về chuỗi token
                }
            }
            return StatusCode(500);
        }
    }
}
