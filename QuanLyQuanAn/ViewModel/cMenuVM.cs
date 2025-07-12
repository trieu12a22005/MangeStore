using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyQuanAn.View;

namespace QuanLyQuanAn.ViewModel
{
    internal class cMenuVM:BaseViewModel
    {
        private object _option = new CatagoryControl();
        private string _selectedOption;

        public object Option { get => _option; set { _option = value; OnPropertyChanged(); } }
        public string SelectedOption { get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();
                switch (_selectedOption)
                {
                    case "Catagory":
                        Option = new CatagoryControl();
                        break;
                    case "Food":
                        Option = new FoodControl();
                        break;
                    default:
                        Option = new CatagoryControl();
                        break;
                }
            }
        }
        public cMenuVM() 
        {
            SelectedOption = "Category";
        }
    }
}
