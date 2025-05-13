using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using QuanLyQuanAn.Model;
using QuanLyQuanAn.View.DialogHost;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Data;
using System.Globalization;

namespace QuanLyQuanAn.ViewModel.MenuVM
{
    internal class FoodControlVM : BaseViewModel
    {
        private object _currentDiaLogContent;
        private string _message;
        private ObservableCollection<FoodShow> _foodList;
        private bool _check;
        private bool _isAllChecked;
        private FoodShow _foodReadyToAdd;
        private ObservableCollection<foodCategory> _catagoryList;
        //
        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
                FilterFoodList(); // Gọi hàm lọc danh sách mỗi khi từ khóa thay đổi
            }
        }

        private ObservableCollection<FoodShow> _filteredFoodList;
        public ObservableCollection<FoodShow> FilteredFoodList
        {
            get => _filteredFoodList;
            set
            {
                _filteredFoodList = value;
                OnPropertyChanged();
            }
        }

        public string TypeAdd{ get; set; }
        public object CurrentDialogContent
        {
            get => _currentDiaLogContent;
            set
            {
                _currentDiaLogContent = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowAddFoodCommand { get; }
        public ICommand CloseAddFood { get; }
        public ICommand AddFood { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DeleteFoods { get; }
        public ICommand FalseCm { get; set; }
        public ICommand TrueCm { get; set; }
        public ICommand AllCheckCm { get; set; }
        public ICommand OpenImageCm { get; set; }
        public ICommand AdjustFood {  get; set; }
        public ObservableCollection<FoodShow> FoodList { get => _foodList; set { _foodList = value; OnPropertyChanged(); } }
        public string Message { get => _message; set { _message = value;OnPropertyChanged(); } }

        public bool IsAllChecked { get => _isAllChecked; set { _isAllChecked = value; OnPropertyChanged(); } }
        public bool Check { get => _check; set { _check = value;OnPropertyChanged(); } }

        public FoodShow FoodReadyToAdd { get
            {
                if (_foodReadyToAdd == null)
                {
                    _foodReadyToAdd = new FoodShow();
                    _foodReadyToAdd.FoodImage = File.ReadAllBytes("EmptyImage.png");
                }
                return _foodReadyToAdd;
            } set { _foodReadyToAdd = value; OnPropertyChanged(); } }

        public ObservableCollection<foodCategory> CatagoryList { get => _catagoryList; set { _catagoryList = value; OnPropertyChanged(); } }

        public FoodControlVM()
        {
            ShowAddFoodCommand = new RelayCommand(async (p)=> { TypeAdd = "Thêm"; CurrentDialogContent = new AddFood(); await ShowDialogContent(); }, (p) => true);
            CloseAddFood = new RelayCommand(
                (p) =>
                {
                    FoodReadyToAdd = null;
                    LoadFood();
                    CloseDialogHost();
                },
                (p) => true
                );
            AddFood = new RelayCommand(
                async (p) =>
                {
                    if (!FoodDataprovider.Food.AddFood(FoodReadyToAdd))
                    {
                        var addFood = CurrentDialogContent;
                        Message = $"Đã có món {FoodReadyToAdd.Name}!";
                        CurrentDialogContent = new Message();
                        CloseDialogHost();
                        await ShowDialogContent();
                        CurrentDialogContent = addFood;
                        await ShowDialogContent();
                    }
                    else
                    {
                        if (FoodReadyToAdd.Id == 0)
                        {
                            Message = $"Thêm thành công món {FoodReadyToAdd.Name}.";
                        }
                        else
                        {
                            Message = $"Món {FoodReadyToAdd.Name} đã được sửa.";
                        }
                        CurrentDialogContent = new Message();
                        CloseDialogHost();
                        await ShowDialogContent();
                        LoadFood();
                        Message = null;
                        FoodReadyToAdd = null;
                    }
                },
                (p)=>
                {
                    if(FoodReadyToAdd.Name == null || FoodReadyToAdd.Name=="" || FoodReadyToAdd.IdCategory==0)
                    {
                        return false;
                    }
                    return true;
                }
                );
            DeleteCommand = new RelayCommand(
                (selectedFood) => DeleteFood(selectedFood),
                (selectedFood) => true // Chỉ kích hoạt nếu món ăn được chọn
            );
            FalseCm = new RelayCommand(
                p =>
                {
                    Check = false;
                    CloseDialogHost();
                },
                p => true
                );
            TrueCm = new RelayCommand(
                p =>
                {
                    Check = true;
                    CloseDialogHost();
                },
                p => true
                );
            DeleteFoods = new RelayCommand(
                async(p) =>
                {
                    var DeleteList = FoodList.Where(l => l.IsChecked == true).ToList();
                    Message = $"Bạn có chắc chắn muốn xóa {DeleteList.Count} món này không?";
                    CurrentDialogContent = new MessageYesNo();
                    await ShowDialogContent();
                    if (Check == true)
                    {
                        foreach(FoodShow food in FoodList)
                        {
                            FoodDataprovider.Food.DeleteFood(food.Id);
                        }
                        LoadFood();
                        IsAllChecked = false;
                    }
                },
                p => FoodList.Any(l => l.IsChecked));
            AllCheckCm = new RelayCommand(
                p =>
                {
                    if (IsAllChecked == true)
                    {
                        foreach (FoodShow food in FilteredFoodList)
                        {
                            food.IsChecked = true;
                        }
                    }
                    else
                    {
                        foreach (FoodShow food in FilteredFoodList)
                        {
                            food.IsChecked = false;
                        }
                    }
                },
                p => true
                );
            OpenImageCm = new RelayCommand(
                p =>
                {
                    OpenFile();
                },
                p => true
                );
            AdjustFood = new RelayCommand(
                async(p) =>
                {
                    if (p is FoodShow food)
                    {
                        FoodReadyToAdd = food;
                        TypeAdd = "Sửa";
                        CurrentDialogContent = new AddFood(); 
                        await ShowDialogContent();
                    }
                },
                p => true);
            LoadFood();
            CatagoryList = new ObservableCollection<foodCategory> (CategoryProvider.Category.GetAllCategory());
        }

        private async Task ShowDialogContent()
        {
             // DialogContent1 là UserControl hoặc nội dung
            await DialogHost.Show(CurrentDialogContent, "RootDialogHost"); // "RootDialogHost" là tên DialogHost Identifier
        }

        private void CloseDialogHost()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async void DeleteFood(Object selectedFood)
        {
            FoodShow food = selectedFood as FoodShow;
            if (food != null)
            {
                // Hiển thị hộp thoại xác nhận
                Message = $"Bạn có chắc chắn muốn xóa món {food.Name} không?";
                CurrentDialogContent = new MessageYesNo();
                await ShowDialogContent();

                if (Check)
                {
                    // Gọi phương thức xóa trong DataProvider
                    FoodDataprovider.Food.DeleteFood(food.Id);
                    // Cập nhật lại danh sách món ăn
                    LoadFood();
                }
            }
        }
        private void LoadFood()
        {
            var foods = FoodDataprovider.Food.GetAllFood();
            FoodList = new ObservableCollection<FoodShow>(foods.Select(p => new FoodShow
            {
                IsChecked = false,
                FoodImage = p.FoodImage,
                Category = p.CategoryName,
                Name = p.name,
                Price = p.price,
                Id = p.idFood,
                IdCategory = p.idFoodCtg
            }));
            FilteredFoodList = new ObservableCollection<FoodShow>(FoodList);
            for (int i = 0; i < FoodList.Count; i++)
            {
                FoodList[i].No = i + 1;
                FoodList[i].CountChecked += ConfirmCheckAll;
            }
        }
        private void ConfirmCheckAll()
        {
            IsAllChecked = !FilteredFoodList.Any(p => p.IsChecked == false);
        }
        private void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FoodReadyToAdd.FoodImage = File.ReadAllBytes(openFileDialog.FileName);
            }
        }
        //
        private void FilterFoodList()
        {
            LoadFood();
            IsAllChecked = false;
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                // Hiển thị toàn bộ món ăn nếu không có từ khóa
                FilteredFoodList = new ObservableCollection<FoodShow>(FoodList);
            }
            else
            {
                // Lọc món ăn dựa trên từ khóa tìm kiếm (không phân biệt chữ hoa/chữ thường)
                var filtered = FoodList.Where(c =>
                    c.Name.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0);

                FilteredFoodList = new ObservableCollection<FoodShow>(filtered);
            }
        }


    }
    public class FoodShow:BaseViewModel
    {
        private int _no;
        private string _name;
        private bool _isChecked;
        private int _price;
        private byte[] _foodImage;
        private string _category;
        private int _id;
        private int _idCategory;
        public event Action CountChecked;
        public int No { get => _no; set { _no = value; OnPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public int Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        public bool IsChecked { get => _isChecked; set { _isChecked = value; OnPropertyChanged(); CountChecked?.Invoke(); } }
        public byte[] FoodImage { get => _foodImage; set { _foodImage = value; OnPropertyChanged(); } }
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); }}
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public int IdCategory { get => _idCategory; set { _idCategory = value; OnPropertyChanged(); } }
    }
    internal class CategoryToId : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is foodCategory category)
            {
                return category.idFoodCtg;
            }
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int idCategory)
            {
                var category = CategoryProvider.Category.GetAllCategory().FirstOrDefault(p => p.idFoodCtg == idCategory);
                return category;
            }
            throw new NotImplementedException();
        }
    }
}
