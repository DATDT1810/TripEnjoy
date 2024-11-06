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
    public class CategoryPageViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand Add { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public CategoryPageViewModel()
        {
            Categories = new ObservableCollection<Category>();
            _ = LoadDataAsync();
            Add = new RelayCommand<Category>(_ => true, async (_) => await ShowAddCatePopup());
            Update = new RelayCommand<Category>(Canchose, async (category) => await UpdateCate(category));
            Delete = new RelayCommand<Category>(Canchose, async (category) => await DeleteCate(category));
        }

        private async Task ShowAddCatePopup()
        {
            var addCategoryViewModel = new AddNewCategoryViewModel();
            var popup = new AddNewCategoryWindow
            {
                DataContext = addCategoryViewModel
            };
            addCategoryViewModel.CloseWindow = result => popup.DialogResult = result;
            if (popup.ShowDialog() == true)
            {
                var newCategory = new Category
                {
                    CategoryName = addCategoryViewModel.CategoryName,
                    CategoryStatus = addCategoryViewModel.CategoryStatus
                };
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(2);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Category");
                var content = new StringContent(JsonConvert.SerializeObject(newCategory), Encoding.UTF8, "application/json");
                request.Content = content;
                var token = TokenHelper.LoadToken();
                if (token == null)
                {
                    throw new ArgumentNullException("Token");
                }
                request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Add category successfully.");
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show("Failed to add category.");
                    await LoadDataAsync();
                }
            }
        }

        private async Task DeleteCate(Category category)
        {
            category = _selectedCategory;
            if (category == null)
            {
                throw new ArgumentNullException("Category");
            }
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Delete, "https://localhost:7126/api/Category/" + category.CategoryId);
            var token = TokenHelper.LoadToken();
            if (token == null)
            {
                throw new ArgumentNullException("Token");
            }
            request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Delete category successfully.");
                await LoadDataAsync();
            }
            else
            {
                MessageBox.Show("Failed to delete category.");
                await LoadDataAsync();
            }
        }

        private async Task UpdateCate(Category category)
        {
            category = category ?? _selectedCategory;
            if (category == null)
            {
                throw new ArgumentNullException("Category");
            }
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7126/api/Category/" + category.CategoryId);
            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            request.Content = content;
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
                await LoadDataAsync();
            }
            else
            {
                MessageBox.Show("Failed to update category.");
                await LoadDataAsync();
            }

        }

        private bool Canchose(Category obj)
        {
            return _selectedCategory != null;
        }

        private async Task LoadDataAsync()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Category");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(content);
                Categories.Clear();
                foreach (var cate in categories)
                {
                    Categories.Add(cate);
                }
            }
            else
            {
                MessageBox.Show("Failed to load data.");
            }
        }
    }
}
