using System.Windows.Controls;
using frontend.ViewModel;

namespace frontend.View
{
    public partial class ScheduleView : UserControl
    {
        public ScheduleView()
        {
            InitializeComponent();
            DataContext = new ScheduleViewModel("DefaultUser");
        }
    }
}