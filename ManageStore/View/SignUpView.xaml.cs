using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManageStore.View
{
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : Window
    {
        public SignUpView()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            //if (txtUsername.Text == "" || txtPassword.Password == "" || txtConfirmPassword.Password == "")
            //{
            //    MessageBox.Show("Please fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else if (txtPassword.Password != txtConfirmPassword.Password)
            //{
            //    MessageBox.Show("Password and Confirm Password do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else
            //{
            //    MessageBox.Show("Sign Up Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //    this.Close();
            //}
        }
    }
}
