using QuanLyQuanAn.Model;
using QuanLyQuanAn.View;
using QuanLyQuanAn.View.StaffView;
using QuanLyQuanAn.View.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyQuanAn.ViewModel
{
    internal class StaffVM:BaseViewModel
    {
        private string _selectedOption;
        private object _option = new OrderFood();
        private bool _isMaximumWindow = false;
        public ICommand LogoutCm { get; set; }
        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                if (value != _selectedOption)
                {
                    _selectedOption = value;
                    OnPropertyChanged();
                    switch (_selectedOption)
                    {
                        case "Order":
                            Option = new OrderFood();
                            break;
                        case "Bill":
                            Option = new Table();
                            break;
                        case "History":
                            Option = new History();
                            break;
                        default:
                            Option = new OrderFood();
                            break;
                    }
                }
            }
        }

        public object Option
        {
            get => _option;
            set
            {
                _option = value;
                OnPropertyChanged();
            }
        }

        public bool IsMaximumWindow { get => _isMaximumWindow; set { _isMaximumWindow = value; OnPropertyChanged(); } }

        public StaffVM()
        {
            LogoutCm = new RelayCommand(
                p=>
                {
                    if (p is Window window)
                    {
                        CurrentAccoutDataprovider.CurrentAccout.LogoutCurrentAccout();
                        var w = new Login();
                        window.Close();
                        w.Show();
                    }
                },
                p=>true
                );
            SelectedOption = "Order";
        }
    }
}
