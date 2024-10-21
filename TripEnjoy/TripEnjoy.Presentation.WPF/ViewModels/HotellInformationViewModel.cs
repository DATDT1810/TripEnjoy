using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Presentation.WPF.ViewModels
{
    public class HotellInformationViewModel : BaseViewModel
    {
        private readonly HttpClient client = null;
        private string api;
		private string categoryApi;

        private Hotel _hotel;
        public Hotel Hotel
        {
            get => this._hotel; set
            {
				this._hotel = value; 
                OnPropertyChanged();
			}
        }

		private List<string> _statusList;
		public List<string> StatusList
		{
			get => _statusList;
			set
			{
				_statusList = value;
				OnPropertyChanged();
			}
		}
		private List<string> _categoryListString;
		public List<string> CategoryListString
		{
			get => _categoryListString;
			set
			{
				_categoryListString = value;
				OnPropertyChanged();
			}
		}

		private string _categorySelected;
		public string CategorySelected
		{
			get => _categorySelected;
			set
			{
				_categorySelected = value;
				OnPropertyChanged();
				if (value != null)
				{
					this.Hotel.CategoryId = getIdByNameCategory(value);
				}
			}
		}

		private IEnumerable<Category> _CategoryList;
		public IEnumerable<Category> CategoryList { get => _CategoryList; set { _CategoryList = value; OnPropertyChanged(); } }

		public ICommand CreateHotelCommand { get; set; }

		public HotellInformationViewModel(int hotelId)
        {
            client = new HttpClient();  
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
			this.api = "https://localhost:7126/api/Hotel";
			this.categoryApi = "https://localhost:7126/api/Category";

            this.StatusList = new List<string>() { "Opening", "Close", "Comming soon", "Under Maintenance"};
			this._categoryListString = new List<string>();
            LoadData(hotelId);
			CreateHotelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				MessageBox.Show(this.Hotel.ToString());
				UpdateHotel();
			});
		}

		private async Task LoadData(int hotelId)
		{
			HttpResponseMessage response = await client.GetAsync(api + "/" + hotelId);
			string data = await response.Content.ReadAsStringAsync();
			var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
			this.Hotel = JsonSerializer.Deserialize<Hotel>(data, option);

			response = await client.GetAsync(this.categoryApi);
			data = await response.Content.ReadAsStringAsync();
			//option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			this.CategoryList = JsonSerializer.Deserialize<IEnumerable<Category>>(data, option);

			foreach (var item in CategoryList)
			{
				this._categoryListString.Add(item.CategoryName);
				if(item.CategoryId == this.Hotel.CategoryId)
				{
					this.CategorySelected = item.CategoryName;
				}
			}
		}

		private int getIdByNameCategory(string categoryname) => this.CategoryList.FirstOrDefault(p => p.CategoryName == categoryname).CategoryId;

		private async Task<Hotel> UpdateHotel()
		{
			string data = JsonSerializer.Serialize(this.Hotel);
			var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PutAsync(this.api+"/"+this.Hotel.HotelId, content);
			if(!response.IsSuccessStatusCode)
			{
				string errorMessage = await response.Content.ReadAsStringAsync();
				MessageBox.Show("Update failed: " + errorMessage);
				return null;
			} else
			{
				MessageBox.Show("update successfull");
				return this.Hotel;
			}
		}
	}
}
