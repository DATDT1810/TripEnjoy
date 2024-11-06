using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TripEnjoy.Presentation.WPF.ViewModels.Admin
{
    public class AddNewCategoryViewModel : BaseViewModel
    {
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        private bool _categoryStatus;
        public bool CategoryStatus
        {
            get => _categoryStatus;
            set
            {
                _categoryStatus = value;
                OnPropertyChanged(nameof(CategoryStatus));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseWindow { get; set; }

        public AddNewCategoryViewModel()
        {
            SaveCommand = new RelayCommand<object>(_ => CanSave(), _ => Save());
            CancelCommand = new RelayCommand<object>(_ => true, _ => Cancel());
        }
        private bool CanSave()
        {
            return !string.IsNullOrEmpty(CategoryName) ;
        }

        private void Save()
        {
            CloseWindow?.Invoke(true); // Đóng cửa sổ và trả về true nếu thành công
        }

        private void Cancel()
        {
            CloseWindow?.Invoke(false); // Đóng cửa sổ và trả về false nếu hủy bỏ
        }
    }
}
