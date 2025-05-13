using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyQuanAn.View.DialogHost;
using QuanLyQuanAn.Model;

namespace QuanLyQuanAn.ViewModel.MenuVM
{

    
    internal class CatagoryControlVM : BaseViewModel
    {
        private object _currentDialogContent;
        private ObservableCollection<CatagoryShow> _categoryList;
        private string _message;
        private bool _check;
        private bool _isAllChecked;
        private CatagoryShow _categoryReadyAdd;
        //thêm
        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
                FilterCategoryList(); // Gọi hàm lọc danh sách mỗi khi từ khóa thay đổi
            }
        }

        private ObservableCollection<CatagoryShow> _filteredCategoryList;
        public ObservableCollection<CatagoryShow> FilteredCategoryList
        {
            get => _filteredCategoryList;
            set
            {
                _filteredCategoryList = value;
                OnPropertyChanged();
            }
        }

        public string TypeAdd { get; set; }
        public object CurrentDialogContent
        {
            get => _currentDialogContent;
            set
            {
                _currentDialogContent = value;
                OnPropertyChanged();
            }
        }
        public ICommand ShowAddCatagoryCommand { get; }
        public ICommand CloseAddCatagory {  get; }
        public ICommand AddCatagory { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DeleteCatagories { get; }
        public ICommand FalseCm { get; set; }
        public ICommand TrueCm { get; set; }
        public ICommand AllCheckCm { get; set; }
        public ICommand AdjustCategory { get; set; }

        public ObservableCollection<CatagoryShow> CategoryList { get => _categoryList; set { _categoryList = value; OnPropertyChanged(); } }

        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }

        public bool Check { get => _check; set { _check = value; OnPropertyChanged(); } }

        public bool IsAllChecked { get => _isAllChecked; set { _isAllChecked = value; OnPropertyChanged(); } }

        public CatagoryShow CategoryReadyAdd { get 
            {
                if (_categoryReadyAdd == null)
                {
                    _categoryReadyAdd = new CatagoryShow();
                }
                return _categoryReadyAdd; 
            } 
            set { _categoryReadyAdd = value; OnPropertyChanged(); } }

        public CatagoryControlVM()
        {
            #region khởi tạo ICommand
            ShowAddCatagoryCommand = new RelayCommand(
                async (p)=>
                {
                    CategoryReadyAdd = null;
                    TypeAdd = "Thêm";
                    CurrentDialogContent = new AddCatagory();
                    await ShowDialogContent(); 
                }, 
                (p)=>true);
            CloseAddCatagory = new RelayCommand(
                (p) =>
                {
                    LoadCategory();
                    CloseDialogHost();
                }
                ,
                (p) => true
                );
            AddCatagory = new RelayCommand(
                async (p) => 
                {
                    if(!CategoryProvider.Category.AddCategory(CategoryReadyAdd))
                    {
                        var addCatagory = CurrentDialogContent;
                        Message = $"Đã có danh mục {CategoryReadyAdd.Name}!";
                        CurrentDialogContent = new Message();
                        CloseDialogHost();
                        await ShowDialogContent();
                        CurrentDialogContent = addCatagory;
                        await ShowDialogContent();
                    }
                    else
                    {
                        if (CategoryReadyAdd.ID == 0)
                        {
                            Message = $"Thêm thành công {CategoryReadyAdd.Name}!";
                        }
                        else
                        {
                            Message = $"Đã sửa danh muc {CategoryReadyAdd.Name}";
                        }
                        CurrentDialogContent = new Message();
                        CloseDialogHost();
                        await ShowDialogContent();
                        LoadCategory();
                        CategoryReadyAdd = null;
                    }  
                },
                (p) => (CategoryReadyAdd.Name != null && CategoryReadyAdd.Name != "")
                );
            FalseCm = new RelayCommand(
                p=>
                {
                    Check = false;
                    CloseDialogHost();
                },
                p=> true
                );
            TrueCm = new RelayCommand(
                p =>
                {
                    Check = true;
                    CloseDialogHost();
                },
                p => true
                );
            DeleteCommand = new RelayCommand(
                async (selectedCategory) =>
                {
                    if (selectedCategory is CatagoryShow category)
                    {
                        Message = $"Bạn có chắc chắn muốn xóa danh mục {category.Name} không?";
                        CurrentDialogContent = new MessageYesNo();
                        await ShowDialogContent();
                        if (Check == true)
                        {
                            if (!CategoryProvider.Category.DeleteCategory(category.ID))
                            {
                                CloseDialogHost();
                                Message = $"Bạn phải xóa hết món ăn có danh mục {category.Name}!";
                                CurrentDialogContent = new Message();
                                await ShowDialogContent();
                            }
                            else
                            {
                                LoadCategory();
                            }
                        }
                    }
                },
                (selectedCategory) => true);
            DeleteCatagories = new RelayCommand(
                async(p) =>
                {
                    var DeleteList = CategoryList.Where(l => l.IsChecked == true).ToList();
                    Message = $"Bạn có chắc chắn muốn xóa {DeleteList.Count} danh mục này không?";
                    CurrentDialogContent = new MessageYesNo();
                    bool Flag = false;
                    await ShowDialogContent();
                    if (Check == true)
                    {
                        Message = "Bạn phải xóa hết những món ăn có danh mục ";
                        for (int i = 0, j=0; i < DeleteList.Count; i++)
                        {
                            if (!CategoryProvider.Category.DeleteCategory(DeleteList[i].ID))
                            {
                                Flag = true;
                                if (j == 0)
                                {
                                    Message += $"{DeleteList[i].Name}";
                                    j++;
                                }
                                else
                                {
                                    Message += $", {DeleteList[i].Name}";
                                }
                            }
                        }
                        if(Flag)
                        {
                            Message += "!";
                            CloseDialogHost();
                            CurrentDialogContent = new Message();
                            await ShowDialogContent();
                        }
                        LoadCategory();
                        IsAllChecked = false;
                    }
                },
                p => CategoryList.Any(l => l.IsChecked)
                );
            AllCheckCm = new RelayCommand(
                p =>
                {
                    if (IsAllChecked==true)
                    {
                        foreach(CatagoryShow catagory in FilteredCategoryList)
                        {
                            catagory.IsChecked = true;
                        }
                    }
                    else
                    {
                        foreach (CatagoryShow catagory in FilteredCategoryList)
                        {
                            catagory.IsChecked = false;
                        }
                    }
                },
                p => true
                );
            AdjustCategory = new RelayCommand(
               async(p)=> 
               {
                   if (p is CatagoryShow category)
                   {
                       CategoryReadyAdd = category;
                       TypeAdd = "Sửa";
                       CurrentDialogContent = new AddCatagory();
                       await ShowDialogContent();
                   }
               });
            #endregion
            LoadCategory();
            // Khởi tạo lệnh xóa
            

        }

        private async Task ShowDialogContent()
        {
            await DialogHost.Show(CurrentDialogContent, "RootDialogHost"); // "RootDialogHost" là tên DialogHost Identifier
        }

        private void CloseDialogHost()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        private void LoadCategory()
        {
            var categories = CategoryProvider.Category.GetAllCategory();
            CategoryList = new ObservableCollection<CatagoryShow>(categories.Select(category => new CatagoryShow
            {
                ID = category.idFoodCtg,
                Name = category.name,
                IsChecked = false
            }));

            // Khởi tạo danh sách lọc
            FilteredCategoryList = new ObservableCollection<CatagoryShow>(CategoryList);

            // Cập nhật số thứ tự và sự kiện kiểm tra
            for (int i = 0; i < CategoryList?.Count; i++)
            {
                CategoryList[i].No = i + 1;
                CategoryList[i].CountChecked += ConfirmCheckAll;
            }
        }

        private void ConfirmCheckAll()
        {
            IsAllChecked = !FilteredCategoryList.Any(p => p.IsChecked == false);
        }
        //thêm
        private void FilterCategoryList()
        {
            LoadCategory();
            IsAllChecked = false;
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                // Hiển thị toàn bộ danh mục nếu không có từ khóa
                FilteredCategoryList = new ObservableCollection<CatagoryShow>(CategoryList);
            }
            else
            {
                // Lọc danh mục dựa trên từ khóa tìm kiếm (không phân biệt chữ hoa/chữ thường)
                var filtered = CategoryList.Where(c =>
                    c.Name.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0);

                FilteredCategoryList = new ObservableCollection<CatagoryShow>(filtered);
            }
        }


    }
    public class CatagoryShow:BaseViewModel
    {
        private int _no;
        private bool _isChecked;
        private string _name;
        public event Action CountChecked;
        public int ID { get; set; }
        public int No { get => _no; set { _no = value; OnPropertyChanged(); } }

        public bool IsChecked { get => _isChecked; set { _isChecked = value; OnPropertyChanged(); CountChecked?.Invoke(); } }

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

    }
}
