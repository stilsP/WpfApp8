using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfApp8.Entities;

namespace WpfApp8.Admin
{
    /// <summary>
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Managers : Window
    {
        public CollectionViewSource _cvs;
        public Dictionary<string, ListSortDirection> _sortDirections = new Dictionary<string, ListSortDirection>();
        public Dictionary<string, SortableGridViewColumnHeader> _headers = new Dictionary<string, SortableGridViewColumnHeader>();
        public List<Users> _users;
        private Users selectedUsers;

        public Managers()
        {
            InitializeComponent();

            // Инициализируем словарь со всеми возможными ключами
            _sortDirections.Add("Name", ListSortDirection.Ascending);
            _sortDirections.Add("Surname", ListSortDirection.Ascending);
            _sortDirections.Add("Patronymic", ListSortDirection.Ascending);
            _sortDirections.Add("BirthDate", ListSortDirection.Ascending);
            _sortDirections.Add("Login", ListSortDirection.Ascending);
            _sortDirections.Add("Password", ListSortDirection.Ascending);
            _sortDirections.Add("Phone", ListSortDirection.Ascending);
            _sortDirections.Add("Adress", ListSortDirection.Ascending);


            _cvs = new CollectionViewSource();
            _cvs.Source = _users;
            LViewUsers.ItemsSource = _cvs.View;
            LViewUsers.ItemsSource = App.Context.Users.ToList();
            LViewUsers.SelectionChanged += LViewUsers_SelectionChanged;

            foreach (GridViewColumn column in (LViewUsers.View as GridView).Columns)
            {
                var header = column.Header as SortableGridViewColumnHeader;
                if (header != null)
                {
                    _headers.Add((string)header.Tag, header);
                    // Теперь это не вызовет ошибку, так как все ключи уже добавлены
                    _sortDirections[(string)header.Tag] = ListSortDirection.Ascending;
                }
            }
        }
        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close(); // Закрываем текущее окно
        }
        public void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = (CollectionView)CollectionViewSource.GetDefaultView(LViewUsers.ItemsSource);
            view.Filter = new Predicate<object>(Filter);
        }
        public bool Filter(object item)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                return true;

            var Users = item as Users;
            return
                   Users.Name.Contains(SearchTextBox.Text) ||
                   Users.Surname.Contains(SearchTextBox.Text) ||
                   Users.Patronymic.Contains(SearchTextBox.Text) ||
                   Users.BirthDate.ToString().Contains(SearchTextBox.Text) ||
                   Users.Login.Contains(SearchTextBox.Text) ||
                   Users.Password.Contains(SearchTextBox.Text) ||
                   Users.Phone.Contains(SearchTextBox.Text) ||
                   Users.Adress.Contains(SearchTextBox.Text);
        }
        public void UpdateSortIndicators()
        {
            // Убираем все индикаторы
            foreach (var header in _headers.Values)
            {
                header.UpdateHeader("");
            }

            // Добавляем текущий индикатор
            var currentColumn = (LViewUsers.View as GridView).Columns.FirstOrDefault(c => c.Header is SortableGridViewColumnHeader header && _sortDirections.ContainsKey((string)header.Tag));

            if (currentColumn != null)
            {
                var header = _headers[(string)((SortableGridViewColumnHeader)currentColumn.Header).Tag];
                string indicator = _sortDirections[(string)((SortableGridViewColumnHeader)currentColumn.Header).Tag] == ListSortDirection.Ascending ? "1" : "2";
                header.UpdateHeader(indicator);
            }
        }

        public void ClearSorting()
        {
            _cvs.SortDescriptions.Clear();
            UpdateSortIndicators();
        }


        public void ClearSortButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSorting();
        }
        public void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string propertyName = (string)selectedItem.Tag;
                SortByColumn(propertyName);
            }
        }
        public void SortByColumn(string propertyName)
        {
            ListSortDirection direction = _sortDirections[propertyName];

            LViewUsers.Items.SortDescriptions.Clear();
            LViewUsers.Items.SortDescriptions.Add(new SortDescription(propertyName, direction));

            _sortDirections[propertyName] = direction == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;

            UpdateSortIndicators();
        }
        private void LViewUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUsers = LViewUsers.SelectedItem as Users;
            EditButton.IsEnabled = selectedUsers != null;
            DeleteButton.IsEnabled = selectedUsers != null;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addManager = new AddManager();
            if (addManager.ShowDialog() == true)
            {
                App.Context.Users.Add(addManager.Users);
                App.Context.SaveChanges();
                LViewUsers.ItemsSource = App.Context.Users.ToList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUsers != null)
            {
                var editWindow = new AddManager(selectedUsers);
                if (editWindow.ShowDialog() == true)
                {
                    // Обновляем данные в базе
                    App.Context.Entry(selectedUsers).CurrentValues.SetValues(editWindow.Users);
                    App.Context.SaveChanges();
                    LViewUsers.ItemsSource = App.Context.Users.ToList();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUsers != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    App.Context.Users.Remove(selectedUsers);
                    App.Context.SaveChanges();
                    LViewUsers.ItemsSource = App.Context.Users.ToList();
                }
            }
        }

    }

}
