using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using TripEnjoy.Presentation.WPF.Views.Dashboard;
using System.Net.Http;
using System.Net.Http.Headers;
using TripEnjoy.Application.Data;
using System.Text.Json;



namespace TripEnjoy.Presentation.WPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient client = null;
        private string api;

        private string _Username;
        public string  Username { get => this._Username; set { this._Username = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => this._Password; set { this._Password = value; OnPropertyChanged(); } }

		private string _ErrorMessage;
		public string ErrorMessage { get => this._ErrorMessage; set { this._ErrorMessage = value; OnPropertyChanged(); } }

		public ICommand CloseWindowCommand { get; set; }
        public ICommand maximizeWindowCommand { get; set; }
        public ICommand minimizeWindowCommand { get; set; }
        public ICommand mouseMoveWindowCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
		public LoginViewModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.api = "https://localhost:7126/api/Account";
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { this.Password = p.Password; });
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
            LoginCommand = new RelayCommand<object>((p) => { return !checkingUsernameAndPassword(); }, async (p) =>
            {
				//DashboardWindow dashboardWindow = new DashboardWindow();
    //            dashboardWindow.Show();
                await LoginAsync();
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

        private bool checkingUsernameAndPassword()
        {
            return string.IsNullOrEmpty(this.Username) || string.IsNullOrEmpty(this.Password);         
        }

        private async Task LoginAsync()
        {
            AccountDTO accountDTO = new AccountDTO()
            {
                email = this.Username,
                password = this.Password,
            };
            String data = JsonSerializer.Serialize(accountDTO);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(api+ "/Login", content);
            if (response.IsSuccessStatusCode) // nhận phản hồi về sau khi gửi dữ liệu đi
            {
				var dataResponse = response.Content.ReadAsStringAsync().Result; // take token
                //MessageBox.Show(dataResponse);
				DashboardWindow dashboardWindow = new DashboardWindow();
                dashboardWindow.Show();

				
			}
			else
            {
                this.ErrorMessage = "Thông tin xác thực đăng nhập của bạn không khớp với tài khoản trong hệ thống";
            }
        }
    }
}
