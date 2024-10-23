using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TripEnjoy.Domain.Models;
using TripEnjoy.Presentation.WPF.Views.HotelManagement;

namespace TripEnjoy.Presentation.WPF.ViewModels
{
    public class HotelOverviewViewModel : BaseViewModel
    {
		private readonly HttpClient client = null;
		private string api;

		private HotelManagementViewModel _hotelManagementViewModel;

		private IEnumerable<Hotel> _Hotels;
		public IEnumerable<Hotel> Hotels { get => this._Hotels; set { this._Hotels = value; OnPropertyChanged(); } }

		public ICommand HotelDetailCommand { get; set; }
		public ICommand CreateHotelPageCommand { get; set; }
		public ICommand DeleteHotelPageCommand { get; set; }

		private bool _IsLoanding;
		public bool IsLoading { get => this._IsLoanding; set { this._IsLoanding = value; OnPropertyChanged(); } }

		private Hotel _SelectedItem;
		public Hotel SelectedItem
		{
			get => this._SelectedItem; set
			{
				this._SelectedItem = value; OnPropertyChanged();
				if (SelectedItem != null)
				{
					//MessageBox.Show("Hotel: " + SelectedItem.ToString());
					HotelDetailPage hotelDetailPage = new HotelDetailPage();
					HotelDetailViewModel hotelDetailViewModel = new HotelDetailViewModel(SelectedItem);
					hotelDetailPage.DataContext = hotelDetailViewModel;
					this._hotelManagementViewModel.CurrentPage = hotelDetailPage;
				}
			}
		}
		public HotelOverviewViewModel(HotelManagementViewModel hotelManagementViewModel)
        {
			this._hotelManagementViewModel = hotelManagementViewModel;
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			this.api = "https://localhost:7126/api/Hotel";
			LoadData();

			HotelDetailCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				MessageBox.Show("hello world!");
			});

			DeleteHotelPageCommand = new RelayCommand<Hotel>((hotel) => { return true; }, (hotel) =>
			{
				//MessageBox.Show("Hotel Id: "+ hotel.HotelName);
				DeleteHotel(hotel);
			});

			CreateHotelPageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				CreateHotelWindow createHotelWindow = new CreateHotelWindow();
				createHotelWindow.ShowDialog();
				var createHotelViewModel = createHotelWindow.DataContext as CreateHotelViewModel;
				if (createHotelViewModel.IsCreateHotel)
				{
					LoadData();
				}
			});
		}

		private async void LoadData()
		{
			IsLoading = true;
			HttpResponseMessage response = await client.GetAsync(api);
			string data = await response.Content.ReadAsStringAsync();
			var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			Hotels = JsonSerializer.Deserialize<IEnumerable<Hotel>>(data, option);
			IsLoading = false;
		}

		public async Task DeleteHotel(Hotel hotel)
		{
			int hotelId = hotel.HotelId;
			HttpResponseMessage response = await client.DeleteAsync(api + "/" + hotelId);
			if (response.IsSuccessStatusCode)
			{
				var data = response.Content.ReadAsStringAsync().Result;
				var hotelResponse = JsonSerializer.Deserialize<Hotel>(data);
				MessageBox.Show("Delete successfull");
				LoadData();
			}
			else
			{
				MessageBox.Show("Delete False");
			}
		}
	}
}
