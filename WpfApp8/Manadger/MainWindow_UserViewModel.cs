using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp8.Manadger
{
    public class MainWindow_UserViewModel : INotifyPropertyChanged
    {
        private string _userFullName = UserSession.CurrentUserFullName;

        public string UserFullName
        {
            get => _userFullName;
            set
            {
                _userFullName = value;
                OnPropertyChanged();
            }
        }

        public MainWindow_UserViewModel()
        {
            // Принудительно обновляем при создании
            UserFullName = UserSession.CurrentUserFullName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}