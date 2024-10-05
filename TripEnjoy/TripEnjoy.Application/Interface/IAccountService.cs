using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;


namespace TripEnjoy.Application.Interface
{
    public interface IAccountService
    {
        Task<TokenResponseDTO> Login(AccountDTO account);
        Task<IdentityUser> Register(AccountDTO account);
        Task<TokenResponseDTO> RefreshToken(string refreshToken);
        Task<bool> Logout(string email);
        Task<TokenResponseDTO> LoginGoogle(string email);
      
        Task<string> CheckEmail(string email);

        Task<bool> ResetPassword(string email, string password);

    }
}
