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
using WpfApp8;
using WpfApp8.Entities;

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для clients.xaml
    /// </summary>
    public partial class clients : Window
    {
        public CollectionViewSource _cvs;
        public Dictionary<string, ListSortDirection> _sortDirections = new Dictionary<string, ListSortDirection>();
        public Dictionary<string, SortableGridViewColumnHeader> _headers = new Dictionary<string, SortableGridViewColumnHeader>();
        public List<Clients> _clients;

        public clients()
        {
            InitializeComponent();

            // Инициализируем словарь со всеми возможными ключами
            _sortDirections.Add("Name", ListSortDirection.Ascending);
            _sortDirections.Add("Surname", ListSortDirection.Ascending);
            _sortDirections.Add("Patronymic", ListSortDirection.Ascending);
            _sortDirections.Add("Adress", ListSortDirection.Ascending);
            _sortDirections.Add("Phone", ListSortDirection.Ascending);
            _sortDirections.Add("email", ListSortDirection.Ascending);

            _cvs = new CollectionViewSource();
            _cvs.Source = _clients;
            LViewClients.ItemsSource = _cvs.View;
            LViewClients.ItemsSource = App.Context.Clients.ToList();

            foreach (GridViewColumn column in (LViewClients.View as GridView).Columns)
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
            var view = (CollectionView)CollectionViewSource.GetDefaultView(LViewClients.ItemsSource);
            view.Filter = new Predicate<object>(Filter);
        }
        public bool Filter(object item)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                return true;

            var Clients = item as Clients;
            return 
                   Clients.Name.Contains(SearchTextBox.Text) ||
                   Clients.Surname.Contains(SearchTextBox.Text) ||
                   Clients.Patronymic.Contains(SearchTextBox.Text) ||
                   Clients.Adress.ToString().Contains(SearchTextBox.Text) ||
                   Clients.Phone.Contains(SearchTextBox.Text) ||
                   Clients.email.ToString().Contains(SearchTextBox.Text);
        }
        public void UpdateSortIndicators()
        {
            // Убираем все индикаторы
            foreach (var header in _headers.Values)
            {
                header.UpdateHeader("");
            }

            // Добавляем текущий индикатор
            var currentColumn = (LViewClients.View as GridView).Columns.FirstOrDefault(c => c.Header is SortableGridViewColumnHeader header && _sortDirections.ContainsKey((string)header.Tag));

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

            LViewClients.Items.SortDescriptions.Clear();
            LViewClients.Items.SortDescriptions.Add(new SortDescription(propertyName, direction));

            _sortDirections[propertyName] = direction == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;

            UpdateSortIndicators();
        }
    }
}
