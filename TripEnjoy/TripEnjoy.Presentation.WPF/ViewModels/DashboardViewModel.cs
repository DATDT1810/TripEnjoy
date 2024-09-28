using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using TripEnjoy.Presentation.WPF.Views.Activity;
using TripEnjoy.Presentation.WPF.Views.HotelManagement;

namespace TripEnjoy.Presentation.WPF.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand maximizeWindowCommand { get; set; }
        public ICommand minimizeWindowCommand { get; set; }
        public ICommand mouseMoveWindowCommand { get; set; }
        public ICommand loadedWindowCommand { get; set; }
        public ICommand HotelManagementPageCommand { get; set; }
        public ICommand ActivityPageCommand { get; set; }
        public DashboardViewModel()
        {
            loadedWindowCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    Debug.WriteLine("Frame is not null");
                    p.Content = new ActivityPage();
                }
                else
                {
                    MessageBox.Show("Frame is null");
                    Debug.WriteLine("Frame is null");
                }
            });
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

            HotelManagementPageCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    Debug.WriteLine("Frame is not null");
                    p.Content = new HotelManagementPage();
                }
                else
                {
                    MessageBox.Show("Frame is null");
                    Debug.WriteLine("Frame is null");
                }
            });
            ActivityPageCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    Debug.WriteLine("Frame is not null");
                    p.Content = new ActivityPage();
                }
                else
                {
                    MessageBox.Show("Frame is null");
                    Debug.WriteLine("Frame is null");
                }
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
    }
}
