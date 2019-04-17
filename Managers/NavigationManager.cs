using Lab5_Mysko.Models;

namespace Lab5_Mysko.Managers
{
        class NavigationManager
        {
        private static NavigationManager _manager;

        private static object _lock = new object();
        private NavigationModel _model;

        public static NavigationManager Instance
        {
            get
            {
                if (_manager == null)
                {
                    lock (_lock)
                    {
                        _manager = new NavigationManager();
                    }
                }

                return _manager;
            }
        }

        public void Initialise(NavigationModel model)
        {
            _model = model;
        }

        public void Navigate(Models.Views view)
        {
            _model?.Navigate(view);
        }
    }
    }
