using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TripEnjoy.Presentation.WPF.Views.Admin;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class AdminDashBoardViewModel
    {
        public ICommand TabSelectedCommand { get; }
        public string SelectedTab { get; set; }
        public Action<Page> NavigateAction { get; set; }
        public AdminDashBoardViewModel()
        {
            TabSelectedCommand = new RelayCommand<object>( CanExecuteTabSelected, OnTabSelected);
        }

        private bool CanExecuteTabSelected(object parameter)
        {
            if (parameter is string tabName)
            {
                return !string.IsNullOrEmpty(tabName); 
            }
            return false;
        }

        private void OnTabSelected(object parameter)
        {
            if (parameter is string tabName)
            {
                SelectedTab = tabName;
                switch (tabName)
                {
                    case "Home":
                        var homePage = new AdminHomePage();
                        NavigateAction?.Invoke(homePage);
                        break;
                    case "Partner":
                        var partnerPage = new PartnerPage();
                        NavigateAction?.Invoke(partnerPage);
                        break;

                    case "Booking" :
                        var bookingPage = new BookingPage();
                        NavigateAction?.Invoke(bookingPage);
                        break;
                    case "Category":
                        var categoryPage = new CategoryPage();
                        NavigateAction?.Invoke(categoryPage);
                        break;  
                }
            }
        }}
    }
