//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using TripEnjoy.Domain.Models;
//using TripEnjoy.Presentation.WPF.Views.HotelManagement;

//namespace TripEnjoy.Presentation.WPF.ViewModels
//{
//    public class HotelDetailViewModel : BaseViewModel
//    {
//		private Hotel _SelectedItem;
//		public Hotel SelectedItem
//		{
//			get => this._SelectedItem; set
//			{
//				this._SelectedItem = value; OnPropertyChanged();
//			}
//		}

//		private Page _CurrentHotelDetailPage;
//		public Page CurrentHotelDetailPage
//		{
//			get => _CurrentHotelDetailPage;
//			set
//			{
//				_CurrentHotelDetailPage = value;
//				OnPropertyChanged();
//			}
//		}

//		public HotelDetailViewModel(Hotel SelectedHotel)
//        {
//            this.SelectedItem = SelectedHotel;
//			HotellInformation hotellInformation = new HotellInformation();
//			HotellInformationViewModel hotellInformationViewModel = new HotellInformationViewModel(this.SelectedItem.HotelId);
//			hotellInformation.DataContext = hotellInformationViewModel;
//			CurrentHotelDetailPage = hotellInformation;
//        }
//    }
//}
