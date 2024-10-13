using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.User;

namespace TripEnjoy.Application.Services.User
{
    public class UserService : IUserServices
    {
        private readonly IAccountRepository accountRepository;

        public UserService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        public Task<UserProfile> CreateUser(UserProfile userProfile)
        {
           return accountRepository.CreateUser(userProfile);
        }

        public Task<bool> DeleteUser(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfile> GetUserProfile(string accountId)
        {
            return accountRepository.GetUserProfile(accountId);
        }

        public Task<UserProfile> UpdateUserProfile(UserProfile userProfile)
        {
            return accountRepository.UpdateUserProfile(userProfile);
        }
    }
}
