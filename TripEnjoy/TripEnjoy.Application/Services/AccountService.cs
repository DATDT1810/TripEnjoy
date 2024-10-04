using Microsoft.AspNetCore.Identity;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;

namespace TripEnjoy.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;


        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        Task<TokenResponseDTO> IAccountService.Login(AccountDTO account)
        {

            return accountRepository.Login(account);
        }

        Task<IdentityUser> IAccountService.Register(AccountDTO account)
        {
            return accountRepository.Register(account);
        }

        public Task<TokenResponseDTO> RefreshToken(string refreshToken)
        {
            return this.accountRepository.RefreshToken(refreshToken);
        }

        public Task<bool> Logout(string email)
        {
           return this.accountRepository.Logout(email);
        }

        public Task<TokenResponseDTO> LoginGoogle(string email)
        {
            return this.accountRepository.LoginGoogle(email);
        }
    }
}
