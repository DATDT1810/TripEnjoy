using Microsoft.AspNetCore.Identity;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Domain.Models;

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

        public Task<TokenResponseDTO> RefreshToken(TokenRefreshDTO refreshToken)
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

        public Task<string> CheckEmail(string email)
        {
            return this.accountRepository.CheckEmail(email);
        }

      

        public Task<bool> ResetPassword(string email, string password)
        {
          return this.accountRepository.ResetPassword(email, password);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await this.accountRepository.GetAllAccountsAsync();
        }

        public async Task<Account> GetAccountByIdAsync(string userId)
        {
            return await this.accountRepository.GetAccountByIdAsync(userId);
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            return await this.accountRepository.AddAccountAsync(account);
        }

        public async Task<Account> UpdateAccountLevelAsync(string UId)
        {
            return await this.accountRepository.UpdateAccountLevelAsync(UId);
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await this.accountRepository.GetAccountByEmailAsync(email);
        }
    }
}
