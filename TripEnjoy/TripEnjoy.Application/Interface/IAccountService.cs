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
    }
}
