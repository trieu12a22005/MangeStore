using QuanLyQuanAn.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyQuanAn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Kiểm tra trạng thái đăng nhập
            var currentAccount = CurrentAccoutDataprovider.CurrentAccout.GetCurrentAccoutByIdMachine();

            if (currentAccount.Count > 0)
            {
                int idAccount = (int)currentAccount[0].idAccount;
                var account = AccountDataprovider.Account.GetAccountById(idAccount);

                string typeAccount = account[0].TypeAccount;

                // Hiển thị cửa sổ phù hợp với loại tài khoản
                if (typeAccount == "Quản lý")
                {
                    Admin adminWindow = new Admin();
                    adminWindow.Show();
                }
                else if (typeAccount == "Nhân viên")
                {
                    Staff staffWindow = new Staff();
                    staffWindow.Show();
                    
                }
                else if (typeAccount == "Đầu bếp")
                {
                    Cheff cheffWindow = new Cheff();
                    cheffWindow.Show();
                }
            }
            else
            {
                Login loginWindow = new Login();
                loginWindow.Show();
            }
        }
    }
}
