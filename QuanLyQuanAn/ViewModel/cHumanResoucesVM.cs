using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using QuanLyQuanAn.Model;
using QuanLyQuanAn.View.DialogHost;
using QuanLyQuanAn.View.HumanResouce;
using QuanLyQuanAn.ViewModel.MenuVM;

namespace QuanLyQuanAn.ViewModel
{
    public class cHumanResoucesVM : TableControlVM
    {
        private readonly int? idThisAccout = CurrentAccoutDataprovider.CurrentAccout.GetCurrentAccoutByIdMachine()[0].idAccount;
        private ObservableCollection<HumanShow> _humanList;
        private object _option;
        private string _selectedOption;
        private HumanShow _humanReadyToAdd;
        public string TypeAddPassword { get; set; }
        public ObservableCollection<HumanShow> HumanList { get => _humanList; set { _humanList = value; OnPropertyChanged(); } }
        public ICommand DeleteHumans { get; set; }
        public ICommand AddHuman { get; }
        public ICommand ShowAddHumanCommand { get; }
        public ICommand AdjustHuman { get; set; }

        public object Option { get => _option; set { _option = value; OnPropertyChanged(); } }
        public string SelectedOption { get => _selectedOption;
            set
            {
                if (value != _selectedOption)
                {
                    _selectedOption = value;
                    OnPropertyChanged();
                    switch (SelectedOption)
                    {
                        case "Manager":
                            Option = new LoadHuman();
                            LoadHumanList();
                            HumanReadyToAdd.TypeAccout = "Quản lý";
                            IsAllChecked = false;
                            break;
                        case "Staff":
                            Option = new LoadHuman();
                            LoadHumanList();
                            HumanReadyToAdd.TypeAccout = "Nhân viên";
                            IsAllChecked = false;
                            break;
                        case "Chef":
                            Option = new LoadHuman();
                            LoadHumanList();
                            HumanReadyToAdd.TypeAccout = "Đầu bếp";
                            IsAllChecked = false;
                            break;
                        default:
                            Option = new LoadHuman();
                            LoadHumanList();
                            HumanReadyToAdd.TypeAccout = "Quản lý";
                            IsAllChecked = false;
                            break;
                    }
                }
            }
        }
        //them
        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
                FilterHumanList(); // Gọi hàm lọc danh sách mỗi khi từ khóa thay đổi
            }
        }

        public HumanShow HumanReadyToAdd {
            get
            {
                if (_humanReadyToAdd == null)
                {
                    _humanReadyToAdd = new HumanShow();
                    switch (_selectedOption)
                    {
                        case "Manager":
                            HumanReadyToAdd.TypeAccout = "Quản lý";
                            break;
                        case "Staff":
                            HumanReadyToAdd.TypeAccout = "Nhân viên";
                            break;
                        case "Chef":
                            HumanReadyToAdd.TypeAccout = "Đầu bếp";
                            break;
                        default:
                            HumanReadyToAdd.TypeAccout = "Quản lý";
                            break;
                    }
                }
                return _humanReadyToAdd;
            }
            set
            {
                _humanReadyToAdd = value;
                OnPropertyChanged();
            }
        }

        public cHumanResoucesVM()
        {
            
            SelectedOption = "Manager";
            DeleteCommand = new RelayCommand(
                async (selectedManager) =>
                {
                    if (selectedManager is HumanShow human)
                    {
                        if (human.IdAccount == idThisAccout)
                        {
                            Message = "Không thể xóa tài khoản bạn đang đăng nhập!";
                            CurrentDialogContent = new Message();
                            CloseDialogHost();
                            await ShowDialogContent();
                        }
                        else
                        {
                            Message = $"Bạn có chắc chắn muốn xóa {human.Name} không?";
                            CurrentDialogContent = new MessageYesNo();
                            await ShowDialogContent();

                            if (Check)
                            {
                                HumanResouceDataProvider.Human.DeleteHuman(human.IdAccount);
                                LoadHumanList();
                            }
                        }
                    }
                },
                (selectedManager) => true);
            DeleteHumans = new RelayCommand(
                async (p) =>
                {
                    var DeleteList = HumanList.Where(l => l.IsChecked == true).ToList();
                    Message = $"Bạn có chắc chắn muốn xóa {DeleteList.Count} người này không?";
                    CurrentDialogContent = new MessageYesNo();
                    await ShowDialogContent();
                    if (Check == true)
                    {
                        if (DeleteList.Any(a => a.IdAccount == idThisAccout))
                        {
                            Message = "Không thể xóa tài khoản bạn đang đăng nhập!";
                            CurrentDialogContent = new Message();
                            CloseDialogHost();
                            await ShowDialogContent();
                        }
                        else
                        {
                            foreach (var human in DeleteList)
                            {
                                HumanResouceDataProvider.Human.DeleteHuman(human.IdAccount);
                            }
                            LoadHumanList();
                            IsAllChecked = false;
                        }
                    }
                },
                p => HumanList.Any(h => h.IsChecked));
            AddHuman = new RelayCommand(
                async (p) =>
                {
                    if(p is PasswordBox password)
                    {
                        HumanReadyToAdd.Password = password.Password;
                        if (!HumanResouceDataProvider.Human.AddHuman(HumanReadyToAdd))
                        {
                            var addHuman = CurrentDialogContent;
                            Message = $"Đã có username {HumanReadyToAdd.Name}!";
                            CurrentDialogContent = new Message();
                            CloseDialogHost();
                            await ShowDialogContent();
                            CurrentDialogContent = addHuman;
                            await ShowDialogContent();
                        }
                        else
                        {
                            if (HumanReadyToAdd.IdAccount == 0)
                            {
                                Message = $"Thêm thành công {HumanReadyToAdd.Name}!";
                            }
                            else
                            {
                                Message = $"Đã sửa username {HumanReadyToAdd.Name}";
                            }
                            CurrentDialogContent = new Message();
                            CloseDialogHost();
                            await ShowDialogContent();
                            LoadHumanList();
                            HumanReadyToAdd = null;
                        }
                    }
                },
                (p) =>
                {
                    if(p is PasswordBox password)
                    {
                        return HumanReadyToAdd.Name != null && HumanReadyToAdd.Name != "" && password.Password != "" && password.Password != null;

                    }
                    return false;
                });
            ShowAddHumanCommand = new RelayCommand(async (p) => 
            {

                TypeAddPassword = "Nhập mật khẩu";
                TypeAdd = "Thêm";
                CurrentDialogContent = new AddAccount(); // DialogContent1 là UserControl hoặc nội dung
                await ShowDialogContent();
            },
            (p) => true);
            AdjustHuman = new RelayCommand(
                async (p) =>
                {
                    if (p is HumanShow human)
                    {
                        TypeAddPassword = "Nhập mật khẩu mới";
                        HumanReadyToAdd = human;
                        TypeAdd = "Sửa";
                        CurrentDialogContent = new AddAccount();
                        await ShowDialogContent();
                    }
                }
                ,
                p => true);
            LoadHumanList();
            InitializeCommands();
            

        }

        private void LoadHumanList()
        {
            if (SelectedOption == "Manager")
            {
                HumanList = new ObservableCollection<HumanShow>(HumanResouceDataProvider.Human.GetHuman("Quản lý").Select(p => 
                new HumanShow
                {
                         IdAccount = p.idAccout,
                         Name = p.Username,
                         IsChecked = false,
                         IdRes = p.idRes,
                         TypeAccout = p.TypeAccount,
                         Password = p.Password
                     }));
            }
            else if (SelectedOption == "Staff")
            {
                HumanList = new ObservableCollection<HumanShow>(HumanResouceDataProvider.Human.GetHuman("Nhân viên").Select(p =>
                new HumanShow
                {
                         IdAccount = p.idAccout,
                         Name = p.Username,
                         Password = p.Password,
                         IdRes = p.idRes,
                         TypeAccout = p.TypeAccount,
                         IsChecked = false
                     }));
            }
            else if (SelectedOption == "Chef")
            {
                HumanList = new ObservableCollection<HumanShow>(HumanResouceDataProvider.Human.GetHuman("Đầu bếp").Select(p =>
                new HumanShow
                {
                         IdAccount = p.idAccout,
                         Name = p.Username,
                         Password = p.Password,
                         IdRes = p.idRes,
                         TypeAccout = p.TypeAccount,
                         IsChecked = false
                     }));
            }
            FilteredList = new ObservableCollection<dynamic>(HumanList);

            for (int i=0; i<HumanList.Count; i++)
            {
                HumanList[i].No = i + 1;
                HumanList[i].CountChecked += ConfirmCheckAll;
            }
        }
        private void FilterHumanList()
        {
            LoadHumanList();
            IsAllChecked = false;
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                // Hiển thị toàn bộ nhân sự nếu không có từ khóa
                FilteredList = new ObservableCollection<dynamic>(HumanList);
            }
            else
            {
                // Lọc nhân sự dựa trên từ khóa tìm kiếm (không phân biệt chữ hoa/chữ thường)
                var filtered = HumanList.Where(c =>
                    c.Name.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0);

                FilteredList = new ObservableCollection<dynamic>(filtered);
            }
        }
    }
    public class HumanShow:BaseViewModel
    {
        private int _no;
        private string _userName;
        private int _idAccount;
        private bool _isChecked;
        public string Password { get; set; }
        public string TypeAccout { get; set; }
        public int IdRes {  get; set; }
        public event Action CountChecked;
        public int No { get => _no; set { _no = value; OnPropertyChanged(); } }
        public string Name { get => _userName; set { _userName = value; OnPropertyChanged(); } }
        public int IdAccount { get => _idAccount; set { _idAccount = value; OnPropertyChanged(); } }
        public bool IsChecked { get => _isChecked; set { _isChecked = value; OnPropertyChanged(); CountChecked?.Invoke(); } }
    }
}
