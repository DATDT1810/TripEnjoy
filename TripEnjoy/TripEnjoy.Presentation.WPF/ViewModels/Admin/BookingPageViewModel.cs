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
using TripEnjoy.Presentation.WPF.Views.Admin;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class BookingPageViewModel : BaseViewModel
    {
        public ObservableCollection<Booking> Bookings { get; set; }


        private Booking _selectedBooking;
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));

            }
        }

        public ICommand ShowDetails { get; }

        public BookingPageViewModel()
        {
            Bookings = new ObservableCollection<Booking>();
            _ = LoadDataAsync();
            ShowDetails = new RelayCommand<Booking>(CanShow, async (_) => await ShowPopupAsync());
        }

        private async Task ShowPopupAsync()
        {
            var popup = new BookingDetailsWindow
            {
                DataContext = SelectedBooking 
            };
            popup.ShowDialog(); // Hiển thị popup
        }

        private bool CanShow(Booking booking)
        {
            return _selectedBooking != null;
        }


        private async Task LoadDataAsync()
          {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Booking");
            var token = TokenHelper.LoadToken();
            request.Headers.Add("Authorization", $"Bearer {token.accessToken}");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bookings = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Booking>>(content);
                Bookings.Clear();
                foreach (var booking in bookings)
                {
                    Bookings.Add(booking);
                }
            }
            throw new Exception("Failed to load data.");
        }
    }
}
