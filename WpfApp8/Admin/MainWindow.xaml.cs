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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp8;
using WpfApp8.Admin;
using WpfApp8.Entities;


namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public CollectionViewSource _cvs;
        public Dictionary<string, ListSortDirection> _sortDirections = new Dictionary<string, ListSortDirection>();
        public Dictionary<string, SortableGridViewColumnHeader> _headers = new Dictionary<string, SortableGridViewColumnHeader>();
        public List<Product> _products;
        private Product selectedProduct;


        public MainWindow()
        {
            InitializeComponent();

            // Инициализируем CollectionViewSource
            _cvs = new CollectionViewSource();
            _cvs.Source = _products;
            LViewServices.ItemsSource = _cvs.View;
            LViewServices.ItemsSource = App.Context.Product.ToList();

            // Сохраняем ссылки на заголовки
            foreach (GridViewColumn column in (LViewServices.View as GridView).Columns)
            {
                var header = column.Header as SortableGridViewColumnHeader;
                if (header != null)
                {
                    _headers.Add((string)header.Tag, header);
                    _sortDirections.Add((string)header.Tag, ListSortDirection.Ascending);
                }
            }
            // Подписываемся на событие выбора элемента
            LViewServices.SelectionChanged += LViewServices_SelectionChanged;
        }
        private void LViewServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProduct = LViewServices.SelectedItem as Product;
            EditButton.IsEnabled = selectedProduct != null;
            DeleteButton.IsEnabled = selectedProduct != null;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditWindow();
            if (addWindow.ShowDialog() == true)
            {
                App.Context.Product.Add(addWindow.Product);
                App.Context.SaveChanges();
                LViewServices.ItemsSource = App.Context.Product.ToList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null)
            {
                var editWindow = new AddEditWindow(selectedProduct);
                if (editWindow.ShowDialog() == true)
                {
                    // Обновляем данные в базе
                    App.Context.Entry(selectedProduct).CurrentValues.SetValues(editWindow.Product);
                    App.Context.SaveChanges();
                    LViewServices.ItemsSource = App.Context.Product.ToList();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    App.Context.Product.Remove(selectedProduct);
                    App.Context.SaveChanges();
                    LViewServices.ItemsSource = App.Context.Product.ToList();
                }
            }
        }



        public void UpdateSortIndicators()
        {
            // Убираем все индикаторы
            foreach (var header in _headers.Values)
            {
                header.UpdateHeader("");
            }

            // Добавляем текущий индикатор
            var currentColumn = (LViewServices.View as GridView).Columns.FirstOrDefault(c =>c.Header is SortableGridViewColumnHeader header &&_sortDirections.ContainsKey((string)header.Tag));

            if (currentColumn != null)
            {
                var header = _headers[(string)((SortableGridViewColumnHeader)currentColumn.Header).Tag];
                string indicator = _sortDirections[(string)((SortableGridViewColumnHeader)currentColumn.Header).Tag] == ListSortDirection.Ascending ? "1" : "2";
                header.UpdateHeader(indicator);
            }
        }

        public void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = (CollectionView)CollectionViewSource.GetDefaultView(LViewServices.ItemsSource);
            view.Filter = new Predicate<object>(Filter);
        }

        public bool Filter(object item)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                return true;

            var product = item as Product;
            return product.Name.Contains(SearchTextBox.Text) ||
                   product.ProductArticle.Contains(SearchTextBox.Text) ||
                   product.Description.Contains(SearchTextBox.Text) ||
                   product.QuantityInStock.ToString().Contains(SearchTextBox.Text) ||
                   product.Measurement.Contains(SearchTextBox.Text) ||
                   product.id_Category.ToString().Contains(SearchTextBox.Text) ||
            product.Cost.ToString().Contains(SearchTextBox.Text);
        }

        // Дополнительный метод для множественной сортировки
        
        public void ClearSorting()
        {
            _cvs.SortDescriptions.Clear();
            UpdateSortIndicators();
        }

        // Обработчик для кнопки очистки сортировки
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

            LViewServices.Items.SortDescriptions.Clear();
            LViewServices.Items.SortDescriptions.Add(new SortDescription(propertyName, direction));

            _sortDirections[propertyName] = direction == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;

            UpdateSortIndicators();
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            clients newWindow = new clients();
            newWindow.Show();
            this.Close();
        }

        private void ManagerButton_Click(object sender, RoutedEventArgs e)
        {
            Managers newWindow = new Managers();
            newWindow.Show();
            this.Close();
        }
    }
}