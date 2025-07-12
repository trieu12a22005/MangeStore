using QuanLyQuanAn.Model;
using QuanLyQuanAn.ViewModel.MenuVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace QuanLyQuanAn.ViewModel
{
    public class ChefVM: TableStaffVM
    {
        private ObservableCollection<BillShow> _listBill = new ObservableCollection<BillShow>();
        private bool _isMaximumWindow = false;
        public bool IsMaximumWindow { get => _isMaximumWindow; set { _isMaximumWindow = value; OnPropertyChanged(); } }
        public ICommand LogoutCm { get; set; }
        public ObservableCollection<BillShow> ListBill { get => _listBill; set { _listBill = value; OnPropertyChanged(); } }

        public ChefVM() 
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
            ShowBillTableCm = new RelayCommand(
                (p) =>
                {
                    if (p is BillShow table)
                    {
                        TypeShow = "Hoàn thành";
                        _currentIdTable = table.IdTable;
                        TotalPrice = BillDataprovider.Bill.GetBillUnpaidByTable(table.IdTable).TotalPrice;
                        BillInfList = new ObservableCollection<ListBillInf>(BillInfDataprovider.BillInf.GetBillInfByTable(table.IdTable));
                        ShowAddFood();
                    }
                },
                (p) =>
                {
                    return true;
                });
            PayBill = new RelayCommand(
                (p) =>
                {
                    BillDataprovider.Bill.CompleteBill(_currentIdTable);
                    CloseDialogHost();
                },
                (p) => true
                );
            loadBill();
            InitializeCommands();
            FilteredList = new ObservableCollection<dynamic>(ListBill);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Đặt khoảng thời gian là 1 giây
            timer.Tick += Timer_Tick; // Đăng ký sự kiện Tick
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            loadBill();
        }
        private void loadBill()
        {
            ObservableCollection<BillShow> CurrentListBill = new ObservableCollection<BillShow>(BillDataprovider.Bill.GetListBillUnpaid().Select(p =>
                new BillShow
                {
                    TableName = p.tableName,
                    PeriodTime = FormatTimeSpan(DateTime.Now - p.TimeIn),
                    IdTable = p.idTable
                }));
            if (!ListBill.SequenceEqual(CurrentListBill))
            {
                ListBill = CurrentListBill;
                for (int i = 0; i < ListBill.Count; i++)
                {
                    ListBill[i].No = i + 1;
                }
            }
        }
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalMinutes < 1)
            {
                return "Dưới 1 phút";
            }

            if (timeSpan.TotalHours < 1)
            {
                return $"{(int)timeSpan.TotalMinutes} phút";
            }

            int hours = (int)timeSpan.TotalHours;
            int minutes = timeSpan.Minutes;

            return minutes > 0 ? $"{hours} giờ {minutes} phút" : $"{hours} giờ";
        }
    }
    public class BillShow:BaseViewModel
    {
        private string _periodTime;
        public int No { get; set; }
        public int IdTable;
        public string TableName { set; get; }
        public override bool Equals(object obj)
        {
            if (obj is BillShow other)
            {
                return No == other.No &&
                       TableName == other.TableName &&
                       PeriodTime == other.PeriodTime; 
            }

            return false;
        }
        public override int GetHashCode()
        {
            int hash = 17; // Giá trị khởi đầu
            hash = hash * 31 + No.GetHashCode();
            hash = hash * 31 + (TableName?.GetHashCode() ?? 0);
            hash = hash * 31 + (PeriodTime?.GetHashCode() ?? 0);
            return hash;
        }

        public string PeriodTime { get => _periodTime; set { _periodTime = value; OnPropertyChanged(); } }
    }
}
