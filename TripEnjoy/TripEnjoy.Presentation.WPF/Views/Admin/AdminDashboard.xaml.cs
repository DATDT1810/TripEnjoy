using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TripEnjoy.Presentation.WPF.ViewModels;

namespace TripEnjoy.Presentation.WPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
            DataContext = new AdminDashBoardViewModel(); // Đảm bảo DataContext được gán
            if (DataContext is AdminDashBoardViewModel adminDashBoardViewModel)
            {
                if (adminDashBoardViewModel.NavigateAction == null)
                {
                    adminDashBoardViewModel.NavigateAction = page => MainFrame.Navigate(page);
                }
            }
        }
    }
}
