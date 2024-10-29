using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TripEnjoy.Presentation.WPF.Helper;
using TripEnjoy.Presentation.WPF.Views.Admin;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class AddNewCategoryViewModel : BaseViewModel
    {
        private string _categoryName;
        private string _categoryStatus;

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        public string CategoryStatus
        {
            get => _categoryStatus;
            set
            {
                _categoryStatus = value;
                OnPropertyChanged(nameof(CategoryStatus));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddNewCategoryViewModel()
        {
            SaveCommand = new RelayCommand<string>(_ => true, async _ => await SaveAsync(_categoryName));
            // CancelCommand = new RelayCommand<string>(CheckValid , async _ => await CancelAsync(_categoryName));
        }

        public bool CheckValid(string categoryName)
        {


            return true;
        }

        private async Task SaveAsync(string categoryName)
        {
            var token = TokenHelper.LoadToken();
            if (token == null)
            {
                throw new Exception();
            }
            categoryName = _categoryName;
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Category/AddCategory");
            request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
            var content = JsonConvert.SerializeObject(categoryName);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Create Successfull");
            }
            else
            {
                MessageBox.Show("Failed to create new category");
            }

            Application.Current.Windows.OfType<AddNewCategoryWindow>().FirstOrDefault()?.Close();
        }

        private async Task CancelAsync(string cateName)
        {
            // Đóng popup mà không lưu
            Application.Current.Windows.OfType<AddNewCategoryWindow>().FirstOrDefault()?.Close();
        }
    }
}
