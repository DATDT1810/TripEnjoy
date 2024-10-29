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
using TripEnjoy.Presentation.WPF.Models;
using TripEnjoy.Presentation.WPF.Views.Admin;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class UpdateCategoryViewModel : BaseViewModel
    {
        public Category Category { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public UpdateCategoryViewModel(Category category)
        {
            Category = category;
            SaveCommand = new RelayCommand<Category>(CheckValid, async _ => await SaveAsync(category));
            CancelCommand = new RelayCommand<Category>(CheckValid, Cancel);
        }
        private bool CheckValid(Category category)
        {
            if (string.IsNullOrEmpty(Category.CategoryName))
            {
                MessageBox.Show("Category name is required");
                return false;
            }
            return true;
        }
        private async Task SaveAsync(Category category)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7126/api/Category/UpdateCategory");
            var content  =  JsonConvert.SerializeObject(category);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var token = TokenHelper.LoadToken();
            if (token == null)
            {
                throw new ArgumentNullException("Token");
            }
            request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Update category successfully.");

            }
            else
            {
                MessageBox.Show("Failed to update category.");
            }
                // Đóng popup và lưu thay đổi nếu cần
                Application.Current.Windows.OfType<UpdateCategoryWindows>().FirstOrDefault()?.Close();
        }

        private void Cancel(Category category)
        {
            // Đóng popup mà không lưu thay đổi
            Application.Current.Windows.OfType<UpdateCategoryWindows>().FirstOrDefault()?.Close();
        }
    }
}
