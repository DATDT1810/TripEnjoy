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
            var popup = new AddNewCategoryWindow
            {
                DataContext = new AddNewCategoryViewModel() // Gán ViewModel làm DataContext
            };
            popup.ShowDialog();
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
            category = _selectedCategory;
            if (category == null)
            {
                throw new ArgumentNullException("Category");
            }
            var editCategoryWindow = new UpdateCategoryWindows
            {
                DataContext = new UpdateCategoryViewModel(category)
            };
            editCategoryWindow.ShowDialog();
            await LoadDataAsync();
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
