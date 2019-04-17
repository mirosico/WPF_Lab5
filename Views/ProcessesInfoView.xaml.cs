using System.Windows.Controls;
using Lab5_Mysko.ViewModels;

namespace Lab5_Mysko.Views
{
    public partial class ProcessesInfoView : UserControl
    {
        private ProcessesInfoViewModel Model { get; }

        public ProcessesInfoView()
        {
            InitializeComponent();
            Model = new ProcessesInfoViewModel();
            DataContext = Model;
        }
    }
}