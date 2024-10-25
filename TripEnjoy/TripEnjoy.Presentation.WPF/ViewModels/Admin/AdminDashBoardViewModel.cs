using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TripEnjoy.Presentation.WPF.Models;
using TripEnjoy.Presentation.WPF.Views.Admin;

namespace TripEnjoy.Presentation.WPF.ViewModels
{
    public class AdminDashBoardViewModel : BaseViewModel
    {

        public ICommand TabSelectedCommand { get; }
        public string SelectedTab { get; set; }
        public Action<Page> NavigateAction { get; set; }
        public AdminDashBoardViewModel()
        {
            TabSelectedCommand = new RelayCommand<object>(OnTabSelected, CanExecuteTabSelected);
        }

        private bool CanExecuteTabSelected(object parameter)
        {
            if (parameter is string tabName)
            {
                return !string.IsNullOrEmpty(tabName); // Chỉ thực thi khi tabName khác null hoặc không rỗng
            }
            return false;
        }

        private void OnTabSelected(object parameter)
        {
            if (parameter is string tabName)
            {
                SelectedTab = tabName;
               switch(tabName)
                {
                    case "Home":
                        var homePage = new HomePage();
                        NavigateAction?.Invoke(homePage);
                        break;

                }
            }
        }


        
    }
}


