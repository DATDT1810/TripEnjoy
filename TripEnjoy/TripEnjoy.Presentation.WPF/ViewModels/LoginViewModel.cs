//using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text.Json;
//using System.Windows;
//using System.Windows.Input;
//using System.Windows.Controls;
//using TripEnjoy.Presentation.WPF.Views.Dashboard;
//using TripEnjoy.Presentation.WPF.Helper;
//using TripEnjoy.Presentation.WPF.Models;
//using System.ComponentModel.DataAnnotations;
//using System.Windows.Media.Imaging;
//using System.Threading.Tasks;
//using TripEnjoy.Presentation.WPF.Views.Admin;
//using Newtonsoft.Json;

//namespace TripEnjoy.Presentation.WPF.ViewModels
//{
//    public class LoginViewModel : BaseViewModel
//    {

//        private string _Username;
//        [Required(ErrorMessage = "Username not be empty.")]
//        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be at least 5 characters.")]
//        public string Username { get => this._Username; set { ValidateProperty(value, "Username"); this._Username = value; OnPropertyChanged(); } }

//        private string _Password;
//        public string Password { get => this._Password; set { this._Password = value; OnPropertyChanged(); } }

//        private string _ErrorMessage;
//        public string ErrorMessage { get => this._ErrorMessage; set { this._ErrorMessage = value; OnPropertyChanged(); } }

//        private bool _IsLoanding;
//        public bool IsLoading { get => this._IsLoanding; set { this._IsLoanding = value; OnPropertyChanged(); } }

//        private BitmapImage _Img;
//        public BitmapImage Img { get => this._Img; set { this._Img = value; OnPropertyChanged(); } }

//        public ICommand CloseWindowCommand { get; set; }
//        public ICommand maximizeWindowCommand { get; set; }
//        public ICommand minimizeWindowCommand { get; set; }
//        public ICommand mouseMoveWindowCommand { get; set; }
//        public ICommand LoginCommand { get; set; }
//        public ICommand PasswordChangedCommand { get; set; }

//        private void ValidateProperty<T>(T value, string name)
//        {
//            Validator.ValidateProperty(value, new ValidationContext(this, null, null)
//            {
//                MemberName = name
//            });
//        }
//        public LoginViewModel()
//        {
//            this.IsLoading = true;
    
//            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { this.Password = p.Password; });
//            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
//            {
//                FrameworkElement window = GetWindowParent(p);
//                var w = window as Window;
//                if (w != null)
//                {
//                    w.Close();
//                }
//            });

//            maximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
//            {
//                FrameworkElement window = GetWindowParent(p);
//                var w = window as Window;
//                if (w != null)
//                {
//                    if (w.WindowState != WindowState.Maximized)
//                    {
//                        w.WindowState = WindowState.Maximized;
//                    }
//                    else
//                    {
//                        w.WindowState = WindowState.Normal;
//                    }
//                }
//            });

//            minimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
//            {
//                FrameworkElement window = GetWindowParent(p);
//                var w = window as Window;
//                if (w != null)
//                {
//                    if (w.WindowState != WindowState.Minimized)
//                    {
//                        w.WindowState = WindowState.Minimized;
//                    }
//                    else
//                    {
//                        w.WindowState = WindowState.Maximized;
//                    }
//                }
//            });

//            mouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
//            {
//                FrameworkElement window = GetWindowParent(p);
//                var w = window as Window;
//                if (w != null)
//                {
//                    w.DragMove();
//                }
//            });
//            LoginCommand = new RelayCommand<Window>((p) => { return !checkingUsernameAndPassword(); }, async (p) =>
//            {
//                await LoginAsync(p);
//            });
//            this.IsLoading = false;
//        }
//        FrameworkElement GetWindowParent(UserControl u)
//        {
//            FrameworkElement parent = u;
//            while (parent.Parent != null)
//            {
//                parent = parent.Parent as FrameworkElement;
//            }
//            return parent;
//        }

//        private bool checkingUsernameAndPassword()
//        {
//            return string.IsNullOrEmpty(this.Username) || string.IsNullOrEmpty(this.Password);
//        }


//        public async Task LoginAsync(Window window)
//        {
//            IsLoading = true;

//            var model = new LoginViewModelRequest
//            {
//                Email = Username.Trim(),
//                Password = Password.Trim(),
//            };

//            if (model == null)
//            {
//                throw new Exception("Model is null");
//            }

//            using var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(2) };
//            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/Login")
//            {
//                Content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json")
//            };

//            var response = await httpClient.SendAsync(request);

//            if (response.IsSuccessStatusCode)
//            {
//                var responseData = await response.Content.ReadAsStringAsync();
//                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseData);

//                if (tokenResponse == null)
//                {
//                    throw new Exception("Token response is null");
//                }

//                TokenService.SaveToken(tokenResponse);

//                var AdminDashboard = new AdminDashboard();
//                AdminDashboard.Show();
//                window.Close();
//            }
//            else
//            {
//                ErrorMessage = "Thông tin xác thực đăng nhập không khớp.";
//                Password = string.Empty; // Xóa mật khẩu nếu đăng nhập không thành công
//            }

//            IsLoading = false;
//        }
//    }
//}
