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
using TripEnjoy.Presentation.WPF.Views.Admin;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class AdminHomeViewModel : BaseViewModel
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public ICommand BlockAccountCommand { get; }
        public ICommand UnblockAccountCommand { get; }
        public ICommand GetDetailAccountCommand { get; }

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
        public AdminHomeViewModel()
        {
            Accounts = new ObservableCollection<Account>();
            _ = LoadDataAsync();
            BlockAccountCommand = new RelayCommand<Account>(CanBlockAccount, async (account) => await BlockAccount(account));
            UnblockAccountCommand = new RelayCommand<Account>(CanRestoreAccount, async (account) => await RestoreAccount(account));
            GetDetailAccountCommand = new RelayCommand<Account>(CanBlockAccount, async (_) => await ShowPopupAsync());
        }

        private async Task LoadDataAsync()
        {
            await GetAccounts();
        }

        private bool CanBlockAccount(Account account)
        {
            return _selectedAccount != null && _selectedAccount.AccountIsDeleted == false;
        }

        private bool CanRestoreAccount(Account account)
        {
            return _selectedAccount != null && _selectedAccount.AccountIsDeleted == true;
        }

        public async Task<ObservableCollection<Account>> GetAccounts()
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(2);
                var token = TokenHelper.LoadToken();
                if (token == null)
                {
                    throw new InvalidOperationException("Token not found.");
                }
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.accessToken}");

                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Account");
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var accounts = JsonConvert.DeserializeObject<List<Account>>(content);

                    if (accounts != null)
                    {
                        Accounts.Clear(); // Clear existing accounts if needed
                        foreach (var account in accounts)
                        {
                            Accounts.Add(account);
                        }
                    }
                }
                else
                {
                    // Xử lý lỗi theo mã trạng thái
                    throw new HttpRequestException($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}");
                }
            }
            return Accounts;
        }

        private async Task ShowPopupAsync()
        {
            var popup = new AccountDetailsWindow
            {
                DataContext = SelectedAccount // Gán tài khoản được chọn làm DataContext
            };
            popup.ShowDialog(); // Hiển thị popup
        }

        private async Task RestoreAccount(Account account)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var token = TokenHelper.LoadToken();
            if (token == null)
            {
                throw new InvalidOperationException("Token not found.");
            }
            client.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {token.accessToken}");
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7126/api/Account/RestoreAccount/" + _selectedAccount.AccountId);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await LoadDataAsync();
                MessageBox.Show("Restore account successfully");
            }
            else
            {
                MessageBox.Show("Restore account failed");
            }
        }

        private async Task BlockAccount(Account account)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var token = TokenHelper.LoadToken();
            if (token == null)
            {
                throw new InvalidOperationException("Token not found.");
            }
            client.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {token.accessToken}");
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7126/api/Account/LockAccount/" + _selectedAccount.AccountId);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await LoadDataAsync();
                MessageBox.Show("Block account successfully");
            }
            else
            {
                MessageBox.Show("Block account failed");
            }
        }
    }
}
