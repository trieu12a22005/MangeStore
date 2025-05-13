using MaterialDesignThemes.Wpf;
using QuanLyQuanAn.Model;
using QuanLyQuanAn.View.DialogHost;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLyQuanAn.ViewModel.MenuVM;

namespace QuanLyQuanAn.ViewModel
{
    public class TableStaffVM:TableControlVM
    {
        private ObservableCollection<ListBillInf> _billInfList;
        private int _totalPrice;
        protected int _currentIdTable;
        private string _typeShow;

        public string TypeShow { get => _typeShow; set { _typeShow = value; OnPropertyChanged(); } }

        public ICommand ShowBillTableCm { get; set; }
        public ICommand CloseShowBillTable { get; set; }
        public ICommand PayBill { get; set; }

        public TableStaffVM()
        {
            ShowBillTableCm = new RelayCommand(
                (p) =>
                {
                    if (p is TableShow table)
                    {
                        TypeShow = "Thanh toán";
                        _currentIdTable = table.IdTable;
                        TotalPrice = BillDataprovider.Bill.GetBillUnpaidByTable(table.IdTable).TotalPrice;
                        BillInfList = new ObservableCollection<ListBillInf>(BillInfDataprovider.BillInf.GetBillInfByTable(table.IdTable));
                        ShowAddFood();
                    }
                }, 
                (p)=>
                {
                    if(p is TableShow table)
                    {
                        if (table.Status == "có người")
                            return true;
                    }
                    return false;
                });
            CloseShowBillTable = new RelayCommand(
                (p) => CloseDialogHost()
                ,
                (p) => true
                );
            PayBill = new RelayCommand(
                async (p) =>
                {
                    if (BillDataprovider.Bill.PayBillByIdTable(_currentIdTable))
                    {
                        Message = "Đã thanh toán thành công!";
                        CurrentDialogContent = new Message();
                        CloseDialogHost();
                        await ShowDialogContent();
                        LoadTable();
                        CloseDialogHost();
                    }
                    else
                    {
                        var addCatagory = CurrentDialogContent;
                        Message = "Không thể thanh toán khi chưa hoàn thành!";
                        CurrentDialogContent = new Message();
                        CloseDialogHost();
                        await ShowDialogContent();
                        CurrentDialogContent = addCatagory;
                        await ShowDialogContent();
                    }
                },
                (p) => true
                );
            DeleteCommand = new RelayCommand(
                async (p)=>
                {
                    if (p is TableShow table)
                    {
                        Message = $"Bạn có chắc chắn muốn xóa bill của bàn {table.Name} không?";
                        CurrentDialogContent = new MessageYesNo();
                        await ShowDialogContent();

                        if (Check)
                        {
                            if (BillDataprovider.Bill.DeleteBill(table.IdTable))
                            {
                                Message = "Đã xóa bill thành công!";
                                CurrentDialogContent = new Message();
                                CloseDialogHost();
                                await ShowDialogContent();
                                LoadTable();
                                CloseDialogHost();
                            }
                            else
                            {
                                Message = "Không thể xóa khi bill đã hoàn thành!";
                                CurrentDialogContent = new Message();
                                CloseDialogHost();
                                await ShowDialogContent();
                            }
                        }
                    }
                },
                (p)=>
                {
                    if (p is TableShow table)
                    {
                        if (table.Status == "có người")
                            return true;
                    }
                    return false ;
                });
            LoadTable();
        }
        protected async void ShowAddFood()
        {
            CurrentDialogContent = new ListBillInfShow(); // DialogContent1 là UserControl hoặc nội dung
            await DialogHost.Show(CurrentDialogContent, "RootDialogHost"); // "RootDialogHost" là tên DialogHost Identifier
        }
        public ObservableCollection<ListBillInf> BillInfList
        {
            get => _billInfList;
            set
            {
                _billInfList = value;
                OnPropertyChanged();
            }
        }

        public int TotalPrice { get => _totalPrice; set { _totalPrice = value;OnPropertyChanged(); } }
    }
}
