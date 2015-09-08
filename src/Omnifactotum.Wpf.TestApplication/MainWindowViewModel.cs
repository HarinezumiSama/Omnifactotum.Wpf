using System;
using System.ComponentModel;
using System.Linq;
using Omnifactotum.Annotations;

namespace Omnifactotum.Wpf.TestApplication
{
    internal sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constants and Fields

        private bool _canMinimize;
        private bool _canMaximize;
        private bool _hasSystemMenu;

        #endregion

        public MainWindowViewModel()
        {
            CanMinimize = true;
            CanMaximize = true;
            HasSystemMenu = true;
        }

        #region Public Properties

        public bool CanMinimize
        {
            get
            {
                return _canMinimize;
            }

            set
            {
                if (value == _canMinimize)
                {
                    return;
                }

                _canMinimize = value;
                OnPropertyChanged(nameof(CanMinimize));
            }
        }

        public bool CanMaximize
        {
            get
            {
                return _canMaximize;
            }

            set
            {
                if (value == _canMaximize)
                {
                    return;
                }

                _canMaximize = value;
                OnPropertyChanged(nameof(CanMaximize));
            }
        }

        public bool HasSystemMenu
        {
            get
            {
                return _hasSystemMenu;
            }

            set
            {
                if (value == _hasSystemMenu)
                {
                    return;
                }

                _hasSystemMenu = value;
                OnPropertyChanged(nameof(HasSystemMenu));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Methods

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}