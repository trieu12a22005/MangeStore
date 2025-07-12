using MaterialDesignThemes.Wpf;
using QuanLyQuanAn.Model;
using QuanLyQuanAn.View.DialogHost;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyQuanAn.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand RegisterCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand CloseWindow {  get; set; }
        public ICommand FalseCm { get; set; }
        public ICommand TrueCm { get; set; }
        public ICommand LoginAccountCommand {  get; set; }
        public ICommand RegisterAccountCm { get; set; }
        public Visibility LoginVisibility { get => loginVisibility; 
            set
                { loginVisibility = value;
                OnPropertyChanged("LoginVisibility");
            } 
        }

        private string _username;
        private string _restaurantName;
        private ComboBoxItem _typeAccount;
        private string _password;
        private string _message;
        private string _restaurantRegister;
        private string _userRegister;
        private bool _check;
        private string _passwordRegister;
        private string _passwordReEnter;
        private object _currentDialogContent;
        public Visibility RegisterVisibility { get => registerVisibility; set { registerVisibility = value; OnPropertyChanged("RegisterVisibility"); } }
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string RestaurantName { get => _restaurantName; set { _restaurantName = value; OnPropertyChanged(); } }
        public ComboBoxItem TypeAccount { get => _typeAccount; set{ _typeAccount = value; OnPropertyChanged(); } }
        public string PasswordRegister { get => _passwordRegister; set { _passwordRegister = value; OnPropertyChanged(); } }
        public string RestaurantRegister { get => _restaurantRegister; set { _restaurantRegister = value; OnPropertyChanged(); } }
        public string UserRegister { get => _userRegister; set { _userRegister = value; OnPropertyChanged(); } }
        public object CurrentDialogContent { get => _currentDialogContent; set { _currentDialogContent = value; OnPropertyChanged(); } }
        public bool Check { get => _check; set { _check = value; OnPropertyChanged(); } }
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }

        public string PasswordReEnter { get => _passwordReEnter; set { _passwordReEnter = value; OnPropertyChanged(); } }

        private Visibility registerVisibility = Visibility.Collapsed;
        private Visibility loginVisibility = Visibility.Visible;
        public LoginViewModel() 
        {
            RegisterCommand = new RelayCommand(
                async (p) =>
                {
                    await Task.Delay(100);
                    LoginVisibility = Visibility.Collapsed;
                    RegisterVisibility = Visibility.Visible;
                },
                (p)=>true
                );
            BackCommand = new RelayCommand(
                async (p) =>
                {
                    await Task.Delay(100);
                    LoginVisibility = Visibility.Visible;
                    RegisterVisibility = Visibility.Collapsed;
                },
                (p) => true
                );
            CloseWindow = new RelayCommand(
                async (p) =>
                {
                    if(p is Window w)
                    {
                        Message = "Bạn có muốn đóng chương trình?";
                        CurrentDialogContent = new MessageYesNo();
                        await ShowDialogContent();
                        if(Check)
                        {
                            w.Close();
                        }
                    }
                },
                (p) => true
                );
            LoginAccountCommand = new RelayCommand(
                async (p) =>
                {
                    if (p is PasswordBox pw)
                    {
                        _password = pw.Password;
                        Window window = Window.GetWindow(pw);
                        if (TypeAccount?.Content is StackPanel TBtypeaccount)
                        {
                            TextBlock tb = TBtypeaccount.Children[1] as TextBlock;
                            if (AccountDataprovider.Account.GetAccountToLogin(RestaurantName, Username, tb.Text, _password).Count > 0)
                            {
                                CurrentAccoutDataprovider.CurrentAccout.UpdateCurrentAccout(RestaurantName, Username);
                                window.Hide();
                                if (tb.Text == "Quản lý")
                                {
                                    Admin w = new Admin();
                                    w.Show();
                                }
                                else if(tb.Text == "Nhân viên")
                                {
                                    Staff w = new Staff(); w.Show();
                                }   
                                else if(tb.Text == "Đầu bếp")
                                {
                                    Cheff w = new Cheff(); w.Show();
                                }
                                try
                                {
                                    window.Close();
                                }
                                catch { }
                            }
                            else
                            {
                                Message = "Thông tin đăng nhập không đúng!";
                                CurrentDialogContent = new Message();
                                await ShowDialogContent();
                            }
                        }
                        else
                        {
                            MessageBox.Show("chưa hoạt động!", "Thông báo");
                        }
                    }
                    else
                    {
                        MessageBox.Show("chưa hoạt động!", "Thông báo");
                    }    
                },
                (p) =>
                {
                    return RestaurantName != null && Username != null && TypeAccount != null && RestaurantName != "" && Username != "";
                });
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
            RegisterAccountCm = new RelayCommand(
                async (p) =>
                {
                    if (p is Grid grid1)
                    {
                        if (PasswordReEnter != PasswordRegister)
                        {
                            Message = "Mật khẩu nhập lại không đúng!";
                            CurrentDialogContent = new Message();
                            await ShowDialogContent();
                            return;
                        }
                        if (AccountDataprovider.Account.RegisterAccout(RestaurantRegister, UserRegister, PasswordRegister))
                        {
                            Message = "Đăng ký thành công!";
                            CurrentDialogContent = new Message();
                            await ShowDialogContent();
                            var window = Window.GetWindow(grid1);
                            CurrentAccoutDataprovider.CurrentAccout.UpdateCurrentAccout(RestaurantRegister, UserRegister);
                            window.Hide();
                            Admin w = new Admin();
                            w.Show();
                            try
                            {
                                window.Close();
                            }
                            catch { }
                        }
                        else
                        {
                            Message = "Tên nhà hàng đã tồn tại!";
                            CurrentDialogContent = new Message();
                            await ShowDialogContent();
                        }
                    }
                },
                (p) =>
                {
                    if (p is Grid grid1)
                    {
                        if (grid1.Children[2] is Grid grid2)
                        {
                            if (grid2.Children[1] is PasswordBox pw)
                            {
                                PasswordRegister = pw.Password;
                            }
                        }
                        if (grid1.Children[3] is Grid grid3)
                        {
                            if (grid3.Children[1] is PasswordBox pw)
                            {
                                PasswordReEnter = pw.Password;
                            }
                        }
                    }
                    return RestaurantRegister?.Length > 0 && UserRegister?.Length > 0 && PasswordRegister?.Length>0 && PasswordReEnter?.Length>0;
                });
        }
        private async Task ShowDialogContent()
        {
            await DialogHost.Show(CurrentDialogContent, "RootDialogHost"); // "RootDialogHost" là tên DialogHost Identifier
        }

        private void CloseDialogHost()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
