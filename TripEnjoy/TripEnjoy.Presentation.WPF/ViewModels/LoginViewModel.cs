using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using TripEnjoy.Presentation.WPF.Views.Dashboard;


namespace TripEnjoy.Presentation.WPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _Username;
        public string  Username { get => this._Username; set { this._Username = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => this._Password; set { this._Password = value; OnPropertyChanged(); } }

        public ICommand CloseWindowCommand { get; set; }
        public ICommand maximizeWindowCommand { get; set; }
        public ICommand minimizeWindowCommand { get; set; }
        public ICommand mouseMoveWindowCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public LoginViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });

            maximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        w.WindowState = WindowState.Normal;
                    }
                }
            });

            minimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != WindowState.Minimized)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                    else
                    {
                        w.WindowState = WindowState.Maximized;
                    }
                }
            });

            mouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
            LoginCommand = new RelayCommand<object>((p) => { return !checkEmptyString(); }, (p) =>
            {
                DashboardWindow dashboardWindow = new DashboardWindow();
                dashboardWindow.Show();
            });
        }
        FrameworkElement GetWindowParent(UserControl u)
        {
            FrameworkElement parent = u;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }

        private bool checkEmptyString()
        {
            if(string.IsNullOrEmpty(this.Username))
            {
                return true;
            }
            return false;
        }
    }
}
