using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyQuanAn.View.Statistics;

namespace QuanLyQuanAn.ViewModel
{
    internal class cStatisticVM:BaseViewModel
    {
        private object _option = new History();
        private string _selectedOption;

        public object Option { get => _option; set { _option = value; OnPropertyChanged(); } }
        public string SelectedOption { get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();
                switch (_selectedOption)
                {
                    case "History":
                        Option = new History();
                        break;
                    case "Revenue":
                        Option = new Revenue();
                        break;
                    case "FoodStatistic":
                        Option = new FoodStatistics();
                        break;
                    default:
                        Option = new History();
                        break;
                }
            }
        }
        public cStatisticVM() 
        {
            SelectedOption = "History";
        }
    }
}
