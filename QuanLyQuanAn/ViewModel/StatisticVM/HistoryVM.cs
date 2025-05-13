using QuanLyQuanAn.Model;  
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace QuanLyQuanAn.ViewModel.StatisticVM
{
    internal class HistoryVM : BaseViewModel
    {
        private ObservableCollection<HistoryItem> _historyList;
        private DateTime _begin;
        private DateTime _end;
        public ObservableCollection<HistoryItem> HistoryList
        {
            get => _historyList;
            set
            {
                _historyList = value;
                OnPropertyChanged();
            }
        }
        public DateTime Begin { get => _begin; set { _begin = value; LoadHistory(); OnPropertyChanged(); } }
        public DateTime End { get => _end; set { _end = value;LoadHistory(); OnPropertyChanged(); } }

        public HistoryVM()
        {
            Begin = DateTime.Now;
            End = DateTime.Now;
            LoadHistory();
        }

        // Phương thức để tải dữ liệu lịch sử
        private void LoadHistory()
        {
            HistoryList = new ObservableCollection<HistoryItem>(BillDataprovider.Bill.GetHistoryByDate(Begin, End.AddDays(1)).Select(p=>
                new HistoryItem
                {
                    TableName = p.tableName,
                    Time = p.TimeOut,
                    Price = p.TotalPrice
                }).ToList());
            for (int i = 0; i < HistoryList.Count; i++)
            {
                HistoryList[i].Order = i + 1;
            }
        }
    }    
    public class HistoryItem:BaseViewModel
    {
        private int _order;
        public string TableName { get; set; }
        public string Time {  get; set; }
        public int Price { get; set; }

        public int Order { get => _order; set { _order = value; OnPropertyChanged(); } }
    }
}
