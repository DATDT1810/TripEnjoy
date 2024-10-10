using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Application.Interface.User
{
    public interface IUserServices
    {
        Task<UserProfile> GetUserProfile(string accountId);
        Task<UserProfile> UpdateUserProfile(UserProfile userProfile);
        Task<UserProfile> CreateUser(UserProfile userProfile);
        Task<bool> DeleteUser(int accountId);
    }
}
