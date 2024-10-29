using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TripEnjoy.Presentation.WPF.Helper;
using TripEnjoy.Presentation.WPF.Models;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class PartnerPageViewModel : BaseViewModel
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public ICommand Accept { get; }
        public ICommand Reject { get; }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));

            }
        }
        public PartnerPageViewModel()
        {
            Accounts = new ObservableCollection<Account>();
            _ = LoadDataAsync();
            Accept = new RelayCommand<Account>(Canchose, async (account) => await AcceptAccount(account));
            Reject = new RelayCommand<Account>(Canchose, async (account) => await RejectAccount(account));
        }

        private async Task RejectAccount(Account account)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7126/api/Account/RejectUpdateAccountLevel/" + _selectedAccount.AccountId);
            var token = TokenHelper.LoadToken();
            request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Reject account level successfully.");
                await LoadDataAsync();
            }
            else
            {
                MessageBox.Show("Failed to reject account level.");
                await LoadDataAsync();
            }
        }

        private async Task AcceptAccount(Account account)
        {
            var cient =  new HttpClient();
            cient.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7126/api/Account/UpgradeLevel/" + _selectedAccount.AccountId);
            var token = TokenHelper.LoadToken();
            request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
            var response = await cient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Update account level successfully.");
                await LoadDataAsync();
            }
            else
            {
                MessageBox.Show("Failed to update account level.");
                await LoadDataAsync();
            }
        }

        private bool Canchose(Account account)
        {
            account = _selectedAccount;
            return account != null;
        }

        private async Task LoadDataAsync()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Account/GetAccountNeedToBecamePartner");
            var token = TokenHelper.LoadToken();
            request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                if (accounts != null)
                {
                    Accounts.Clear();
                    foreach (var account in accounts)
                    {
                        Accounts.Add(account);
                    }
                }
            }
            else
            {
                MessageBox.Show("Not Account need to confirm");
            }
        }
    }
}
