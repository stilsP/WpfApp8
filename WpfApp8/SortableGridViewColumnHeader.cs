using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp8
{
    public class SortableGridViewColumnHeader : GridViewColumnHeader
    {
        public string _originalHeader;
        public string _currentHeader;

        public SortableGridViewColumnHeader()
        {
            AddHandler(ClickEvent, new RoutedEventHandler(OnClick));
        }

        public void OnClick(object sender, RoutedEventArgs e)
        {
            if (Tag != null)
            {
                var propertyName = (string)Tag;
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Dispatcher.Invoke(() => mainWindow.SortByColumn(propertyName));
                }
            }
        }

        public string OriginalHeader
        {
            get { return _originalHeader; }
            set
            {
                _originalHeader = value;
                _currentHeader = value;
                Content = _currentHeader;
            }
        }

        public void UpdateHeader(string indicator)
        {
            _currentHeader = $"{_originalHeader} {indicator}";
            Content = _currentHeader;
        }
    }
}


