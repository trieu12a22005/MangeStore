using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyQuanAn.Model;
using LiveCharts;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics.Eventing.Reader;
using System.Windows;

namespace QuanLyQuanAn.ViewModel.StatisticVM
{
    public class RevenuaVM:FoodStatisticsVM
    {
        private object _timeLable;
        public Func<double, string> Formatter { get; set; }
        private ChartValues<double> _revenueData;

        public RevenuaVM()
        {
            // Khởi tạo Formatter để định dạng số tiền
            Formatter = value => value.ToString("N0");
            TypeRevenua = "Hôm nay";
        }
        public object TimeLable
        {
            get => _timeLable;
            set
            {
                _timeLable = value;
                OnPropertyChanged();
            }
        }

        public ChartValues<double>  RevenueData
        {
            get => _revenueData;
            set
            {
                _revenueData = value;
                OnPropertyChanged();
            }
        }
        protected override void ShowStatistic()
        {
            int PeriodOfTime = (End.AddDays(1) - Begin).Days;
            var Bills = (BillDataprovider.Bill.GetBillByDate(Begin, End.AddDays(1)) as IEnumerable<dynamic>)?
                        .Select(b => (ThoiGian: b.ThoiGian, TotalPrice: (double)b.TongDoanhThu));
            if (Bills.Count() > 0)
            {
                ShowChart = Visibility.Visible;
                if (PeriodOfTime <= 2)
                {
                    TimeLable = Bills?.Select(d => $"{(int)d.ThoiGian}:00").ToList();
                }
                else if (PeriodOfTime > 2 && PeriodOfTime <= 60)
                {
                    TimeLable = Bills?.Select(d => ((DateTime)d.ThoiGian).ToShortDateString()).ToList();
                }
                else if (PeriodOfTime > 60 && PeriodOfTime <= 730)
                {
                    TimeLable = Bills?.Select(d => ((int)d.ThoiGian).ToString()).ToList();
                }
                else
                {
                    TimeLable = Bills?.Select(d => ((int)d.ThoiGian).ToString()).ToList();
                }
                RevenueData = new ChartValues<double>(Bills?.Select(d => d.TotalPrice) ?? Enumerable.Empty<double>());
            }
            else
            {
                ShowChart = Visibility.Hidden;
            }
        }
    }
    public class WidthOfDatePickerAcrossGrid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value*2/5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
