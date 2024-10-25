using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TripEnjoy.Presentation.WPF.Views.Dashboard;
using TripEnjoy.Presentation.WPF.Views.HotelManagement;

namespace TripEnjoy.Presentation.WPF.ViewModels
{
	public class HotelManagementViewModel : BaseViewModel
	{
		private Page _currentPage;
		public Page CurrentPage
		{
			get => _currentPage;
			set
			{
				_currentPage = value;
				OnPropertyChanged();
			}
		}

		public HotelManagementViewModel()
		{
			HotelOverviewPage hotelOverviewPage = new HotelOverviewPage();
			hotelOverviewPage.DataContext = new HotelOverviewViewModel(this);
			CurrentPage = hotelOverviewPage;
		}
	}
}
