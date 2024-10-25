using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripEnjoy.Presentation.WPF.Helper;
using TripEnjoy.Presentation.WPF.Models;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class AccountListViewModel : BaseViewModel
    {
        public ObservableCollection<Account> Accounts { get; set; }
      
       
        public AccountListViewModel()
        {
            GetAccounts();
        }

        public ObservableCollection<Account> GetAccounts()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);
            var token = TokenService.LoadToken();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.accessToken}");
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Accounts");
            var response = client.SendAsync(request);
            if(response.Result.IsSuccessStatusCode)
            {
                var content = response.Result.Content.ReadAsStringAsync();
                var accounts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Account>>(content.Result);
                foreach (var account in accounts)
                {
                    Accounts.Add(account);
                }
            }
            return Accounts;
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }

        public void DeleteAccount(Account account)
        {
            Accounts.Remove(account);
        }

    }
}
