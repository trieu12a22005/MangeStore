using MaterialDesignThemes.Wpf;
using QuanLyQuanAn.ViewModel;
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

namespace QuanLyQuanAn
{
    /// <summary>
    /// Interaction logic for Cheff.xaml
    /// </summary>
    public partial class Cheff : Window
    {
        public Cheff()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is ChefVM VM && VM.IsMaximumWindow == false)
            {
                WindowState = WindowState.Normal;
                var workingArea = SystemParameters.WorkArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                MaximizeButton.Content = new PackIcon { Kind = PackIconKind.WindowRestore };
                VM.IsMaximumWindow = true;
            }
            else if (this.DataContext is ChefVM VM1)
            {
                WindowState = WindowState.Normal;
                this.Left = 100;
                this.Top = 100;
                this.Width = 800;
                this.Height = 450;
                MaximizeButton.Content = new PackIcon { Kind = PackIconKind.WindowMaximize };
                VM1.IsMaximumWindow = false;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
