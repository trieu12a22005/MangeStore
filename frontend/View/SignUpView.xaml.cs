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

namespace frontend.View
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
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }     
        private void textName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtName.Focus();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text) && txtName.Text.Length > 0)
            {
                textName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textName.Visibility = Visibility.Visible;
            }
        }
        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }
        private void textConfirmPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }
        private void txtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConfirmPassword.Password) && txtConfirmPassword.Password.Length > 0)
            {
                textConfirmPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textConfirmPassword.Visibility = Visibility.Visible;
            }
        }
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {

        }        
        private void txtBirth_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "DD/MM/YYYY")
                {
                    textBox.Text = "";
                }
                textBirth.Visibility = Visibility.Collapsed; // Ẩn placeholder khi focus
            }
        }

        private void txtBirth_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = "DD/MM/YYYY";
                    textBirth.Visibility = Visibility.Visible; // Hiện placeholder khi trống
                }
            }
        }

        private void txtBirth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox) // Ensure sender is a TextBox
            {
                string currentText = textBox.Text;

                // Only allow numeric input
                if (!char.IsDigit(e.Text, 0))
                {
                    e.Handled = true;
                    return;
                }

                // Limit maximum length (8 digits for DDMMYYYY)
                int digitCount = currentText.Count(char.IsDigit);
                if (digitCount >= 8)
                {
                    e.Handled = true;
                }
            }
        }
        private bool isUpdatingText = false;
        private void txtBirth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox || isUpdatingText) return;

            try
            {
                isUpdatingText = true;

                string text = textBox.Text;
                int caretIndex = textBox.CaretIndex;

                // Extract digits
                string digits = new string(text.Where(char.IsDigit).ToArray());

                // Format as DD/MM/YYYY
                string formatted = "";
                for (int i = 0; i < digits.Length && i < 8; i++)
                {
                    if (i == 2 || i == 4)
                        formatted += "/";
                    formatted += digits[i];
                }

                // Update text
                textBox.Text = formatted;
                textBirth.Visibility = string.IsNullOrEmpty(formatted) ? Visibility.Visible : Visibility.Collapsed;

                // Calculate caret position
                int newCaretIndex = caretIndex;
                if (caretIndex == 3 || caretIndex == 6) // After adding '/'
                    newCaretIndex++;
                textBox.CaretIndex = Math.Min(newCaretIndex, formatted.Length);
            }
            finally
            {
                isUpdatingText = false;
            }
        }

        private void textBirth_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtBirth.Focus();
        }
    }
}
