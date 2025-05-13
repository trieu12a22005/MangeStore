using QuanLyQuanAn.Model;
using QuanLyQuanAn.View;
using System.Windows;
using System.Windows.Input;

namespace QuanLyQuanAn.ViewModel
{
    internal class AdminVM : BaseViewModel
    {
        private string _selectedOption;
        private object _option=new Menu();
        private bool _isMaximumWindow = false;
        public ICommand LogoutCm { get; set; }
        public string SelectedOption { get => _selectedOption; 
            set 
            {
                if (value != _selectedOption)
                {
                    _selectedOption = value;
                    OnPropertyChanged();
                    switch (_selectedOption)
                    {
                        case "Menu":
                            Option = new Menu();
                            break;
                        case "FoodTable":
                            Option = new TableControl();
                            break;
                        case "HumanResouces":
                            Option = new HumanResources();
                            break;
                        case "Statistic":
                            Option = new StatisticsControl();
                            break;
                        default:
                            Option = new Menu();
                            break;
                    }
                }
            }
        }
        public object Option { get => _option;
            set 
            { 
                _option = value;
                OnPropertyChanged();
            }
        }

        public bool IsMaximumWindow { get => _isMaximumWindow; set { _isMaximumWindow = value; OnPropertyChanged(); } }

        public AdminVM()
        {
            LogoutCm = new RelayCommand(
                p =>
                {
                    if (p is Window window)
                    {
                        CurrentAccoutDataprovider.CurrentAccout.LogoutCurrentAccout();
                        var w = new Login();
                        window.Close();
                        w.Show();
                    }
                },
                p => true
                );
            SelectedOption = "Menu";
        }
    }
}
