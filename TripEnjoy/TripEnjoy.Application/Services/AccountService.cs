using Microsoft.AspNetCore.Identity;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;

namespace TripEnjoy.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
       

        public AccountService(IAccountRepository accountRepository )
        {
            this.accountRepository = accountRepository;
        }

        Task<string> IAccountService.Login(AccountDTO account)
        {
         
            return accountRepository.Login(account);
        }

        Task<IdentityUser> IAccountService.Register(AccountDTO account)
        {
            return accountRepository.Register(account);
        }
    }
}
