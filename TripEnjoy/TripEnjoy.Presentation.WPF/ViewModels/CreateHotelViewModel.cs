//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;
//using TripEnjoy.Domain.Models;
//using TripEnjoy.Presentation.WPF.Helper;
//using TripEnjoy.Presentation.WPF.Models;

//namespace TripEnjoy.Presentation.WPF.ViewModels
//{
//	public class CreateHotelViewModel : BaseViewModel
//	{
//		private readonly HttpClient client = null;
//		private string CategoryApi;
//		private string HotelApi;
//		private string UserApi;

//		private bool _IsCreateHotel = false;
//		public bool IsCreateHotel { get => _IsCreateHotel; set { _IsCreateHotel = value; OnPropertyChanged(); } }

//		public ICommand CloseCommand { get; set; }
//		public ICommand CreateHotelCommand { get; set; }

//		private string _HotelName;
//		public string HotelName { get => _HotelName; set { _HotelName = value; OnPropertyChanged(); } }

//		private string _Address;
//		public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }
//		private string _PhoneNumber;
//		public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }
//		private string _Description;
//		public string Description { get => _Description; set { _Description = value; OnPropertyChanged(); } }
//		private string _Status;
//		public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }
//		private string _Category;
//		public string Category { get => _Category; set { _Category = value; OnPropertyChanged(); } }
//		private DateTime _HotelTimeStart;
//		public DateTime HotelTimeStart { get => _HotelTimeStart; set { _HotelTimeStart = value; OnPropertyChanged(); } }
//		private DateTime _HotelTimeEnd;
//		public DateTime HotelTimeEnd { get => _HotelTimeEnd; set { _HotelTimeEnd = value; OnPropertyChanged(); } }



//		private IEnumerable<Category> _CategoryList;
//		public IEnumerable<Category> CategoryList { get => _CategoryList; set { _CategoryList = value; OnPropertyChanged(); } }
//		public CreateHotelViewModel()
//		{
//			client = new HttpClient();
//			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
//			client.DefaultRequestHeaders.Accept.Add(contentType);
//			this.HotelApi = "https://localhost:7126/api/Hotel";
//			this.CategoryApi = "https://localhost:7126/api/Category";
//			this.UserApi = "https://localhost:7126/api/User";

//			HotelTimeStart = DateTime.Now;
//			HotelTimeEnd = DateTime.Now;

//			CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
//			{
//				p.Close();
//			});
//			CreateHotelCommand = new RelayCommand<Window>(p => true, async p =>
//			{
//				Hotel hotel = await AddHotel();
//				if (hotel == null)
//				{
//					MessageBox.Show("add hotell fail");
//				}
//				else
//				{
//					this.IsCreateHotel = true;
//					p.Close();
//				}
//			});
//			LoadData();
//		}

//		private async Task<Hotel> AddHotel()
//		{
//			string token = CookieStorage.Instance.Get("accessToken");
//			if (string.IsNullOrEmpty(token))
//			{
//				MessageBox.Show("Access token is missing. Please log in again.");
//				return null;
//			}
//			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//			HttpResponseMessage response = await client.GetAsync(this.UserApi+ "/GetUserProfile");
//			string data = await response.Content.ReadAsStringAsync();
//			var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//			var userProfiles = JsonSerializer.Deserialize<UserProfile>(data, option);

//			Hotel hotel = new Hotel(0, HotelName, Address, PhoneNumber, Description, false, Status, HotelTimeStart, HotelTimeEnd, userProfiles.accountId, getCategoryIdByName(Category));
//			data = JsonSerializer.Serialize(hotel);
//			var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
//			response = await client.PostAsync(HotelApi, content);
//			string responseContent = await response.Content.ReadAsStringAsync(); // Lấy nội dung phản hồi

//			if (response.StatusCode == System.Net.HttpStatusCode.OK)
//			{
//				return hotel;
//			}
//			else
//			{
//				MessageBox.Show("Error: " + response.StatusCode + " - " + responseContent); // Hiển thị lỗi chi tiết
//				return null;
//			}
//		}

//		private async void LoadData()
//		{
//			HttpResponseMessage response = await client.GetAsync(this.CategoryApi);
//			string data = await response.Content.ReadAsStringAsync();
//			var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//			this.CategoryList = JsonSerializer.Deserialize<IEnumerable<Category>>(data, option);
//		}

//		private int getCategoryIdByName(string ctegoryName)
//		{
//			return this.CategoryList.FirstOrDefault(p => p.CategoryName == ctegoryName).CategoryId;
//		}
//	}
//}
