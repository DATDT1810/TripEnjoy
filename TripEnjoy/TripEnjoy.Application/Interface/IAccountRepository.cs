using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;


namespace TripEnjoy.Application.Interface
{
    public interface IAccountRepository
    {
        Task<string> Login(AccountDTO account);
        Task<bool> Register(AccountDTO account);
    }
}
