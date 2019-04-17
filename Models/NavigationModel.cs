using System;
using Lab5_Mysko.Views;

namespace Lab5_Mysko.Models
{
    public enum Views
    {
        ProcessesView
    }

    class NavigationModel
    {
        private MainWindow _window;
        private ProcessesInfoView _processesInfoView;

        public NavigationModel(MainWindow window)
        {
            _window = window;
            _processesInfoView = new ProcessesInfoView();
        }

        public void Navigate(Views view)
        {
            switch (view)
            {
                case Views.ProcessesView:
                    _window.MinHeight = 200;
                    _window.MaxHeight = 500;
                    _window.MinWidth = 750;
                    _window.MaxWidth = 900;
                    _window.WindowContent.Content = _processesInfoView;
                    break;
                default:
                    throw new ArgumentException("Inappropriate parameter for navigation !");
            }
        }
    }
}