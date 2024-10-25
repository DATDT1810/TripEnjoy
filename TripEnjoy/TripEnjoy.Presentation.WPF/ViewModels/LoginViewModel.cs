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
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net;
using TripEnjoy.Presentation.WPF.Helper;
using TripEnjoy.Presentation.WPF.Models;
using System.ComponentModel.DataAnnotations;



namespace TripEnjoy.Presentation.WPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient client = null;
        private string api;

		private string _Username;
        [Required(ErrorMessage ="Username not be empty.")]
        [StringLength(50, MinimumLength =2, ErrorMessage ="Username must be at least 5 characters.")]
        public string  Username { get => this._Username; set { ValidateProperty(value, "Username"); this._Username = value;OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => this._Password; set { this._Password = value; OnPropertyChanged(); } }

		private string _ErrorMessage;
		public string ErrorMessage { get => this._ErrorMessage; set { this._ErrorMessage = value; OnPropertyChanged(); } }

		private bool _IsLoanding;
		public bool IsLoading { get => this._IsLoanding; set { this._IsLoanding = value; OnPropertyChanged(); } }

		private BitmapImage _Img;
		public BitmapImage Img { get => this._Img; set { this._Img = value; OnPropertyChanged(); } }

		public ICommand CloseWindowCommand { get; set; }
        public ICommand maximizeWindowCommand { get; set; }
        public ICommand minimizeWindowCommand { get; set; }
        public ICommand mouseMoveWindowCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        private void ValidateProperty<T>(T value, string name)
        {
            Validator.ValidateProperty(value, new ValidationContext(this, null, null){
                MemberName = name
            });
        }
		public LoginViewModel()
        {
            this.IsLoading = true;
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
            LoginCommand = new RelayCommand<Window>((p) => { return !checkingUsernameAndPassword(); }, async (p) =>
            {
                await LoginAsync(p);
            });
            this.IsLoading = false;
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

        private async Task LoginAsync(Window p)
        {
			this.IsLoading = true;
			AccountDTO accountDTO = new AccountDTO()
            {
                email = this.Username.Trim(),
                password = this.Password.Trim(),
            };
            String data = JsonSerializer.Serialize(accountDTO);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(api+ "/Login", content);
            if (response.IsSuccessStatusCode) // nhận phản hồi về sau khi gửi dữ liệu đi
            {
				var responseData = response.Content.ReadAsStringAsync().Result; // take token
                TokenResponse? tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData);
                CookieStorage.Instance.Append("accessToken", tokenResponse.accessToken);
                CookieStorage.Instance.Append("refreshToken", tokenResponse.refreshToken);

				DashboardWindow dashboardWindow = new DashboardWindow();
                dashboardWindow.Show();
                p.Close();
			}
			else
            {
                this.ErrorMessage = "Thông tin xác thực đăng nhập của bạn không khớp với tài khoản trong hệ thống";
                this.Password="";
            }
			this.IsLoading = false;
		}
	}
}
