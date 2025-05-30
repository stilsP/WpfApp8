using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
using WpfApp8.Manadger;
using System.IO; // Для Path.Combine (если будешь использовать)

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow_User.xaml
    /// </summary>
    public partial class MainWindow_User : Window
    {
        public CollectionViewSource _cvs;
        public Dictionary<string, ListSortDirection> _sortDirections = new Dictionary<string, ListSortDirection>();
        public Dictionary<string, SortableGridViewColumnHeader> _headers = new Dictionary<string, SortableGridViewColumnHeader>();
        public List<Order> _order;
        private Order selectedOrder;

        public MainWindow_User()
        {
            InitializeComponent();
            QuestPDF.Settings.License = LicenseType.Community;
            // Инициализируем словарь со всеми возможными ключами
            _sortDirections.Add("Code", ListSortDirection.Ascending);
            _sortDirections.Add("Status", ListSortDirection.Ascending);
            _sortDirections.Add("Date", ListSortDirection.Ascending);
            _sortDirections.Add("id_Client", ListSortDirection.Ascending);
            _sortDirections.Add("ProductArticle", ListSortDirection.Ascending);
            _sortDirections.Add("Quantity", ListSortDirection.Ascending);
            _sortDirections.Add("Manager", ListSortDirection.Ascending);


            _cvs = new CollectionViewSource();
            _cvs.Source = _order;
            LViewOrder.ItemsSource = _cvs.View;
            LViewOrder.ItemsSource = App.Context.Order.ToList();

            foreach (GridViewColumn column in (LViewOrder.View as GridView).Columns)
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
        public void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = (CollectionView)CollectionViewSource.GetDefaultView(LViewOrder.ItemsSource);
            view.Filter = new Predicate<object>(Filter);
        }
        public bool Filter(object item)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
                return true;

            var Order = item as Order;
            return
                   Order.Code.ToString().Contains(SearchTextBox.Text) ||
                   Order.Status.Contains(SearchTextBox.Text) ||
                   Order.Date.ToString().Contains(SearchTextBox.Text) ||
                   Order.id_Client.ToString().Contains(SearchTextBox.Text) ||
                   Order.ProductArticle.Contains(SearchTextBox.Text) ||
                   Order.Quantity.Contains(SearchTextBox.Text) ||
                   Order.ManagerId.ToString().Contains(SearchTextBox.Text);
        }
        public void UpdateSortIndicators()
        {
            // Убираем все индикаторы
            foreach (var header in _headers.Values)
            {
                header.UpdateHeader("");
            }

            // Добавляем текущий индикатор
            var currentColumn = (LViewOrder.View as GridView).Columns.FirstOrDefault(c => c.Header is SortableGridViewColumnHeader header && _sortDirections.ContainsKey((string)header.Tag));

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

            LViewOrder.Items.SortDescriptions.Clear();
            LViewOrder.Items.SortDescriptions.Add(new SortDescription(propertyName, direction));

            _sortDirections[propertyName] = direction == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;

            UpdateSortIndicators();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addOrder = new AddOrder();
            if (addOrder.ShowDialog() == true)
            {
                App.Context.Order.Add(addOrder.Order);
                App.Context.SaveChanges();
                LViewOrder.ItemsSource = App.Context.Order.ToList();
            }
        }

        private void CatalogButton_Click(object sender, RoutedEventArgs e)
        {
            Catalog newWindow = new Catalog();
            newWindow.ShowDialog();
            this.Close();
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            clients newWindow = new clients();
            newWindow.ShowDialog();
            this.Close();
        }
        private void GenerateReceipt_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный заказ
            var selectedOrder = LViewOrder.SelectedItem as Order; // Замените YourDataGrid на имя вашего элемента управления

            if (selectedOrder != null)
            {
                using (var db = new diplomchikEntities())
                {
                    try
                    {
                        // Получаем данные о менеджере и клиенте
                        var order = db.Order
                            .Include("Users")
                            .Include("Clients")
                            .FirstOrDefault(o => o.Code == selectedOrder.Code);

                        if (order != null && order.Users != null)
                        {
                            // Вызываем метод генерации
                            PdfService.GenerateReceipt(order, order.Users, order.Product);
                        }
                        else
                        {
                            MessageBox.Show("Нет данных для формирования чека!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для формирования чека!");
            }
        }
        public class AppDbContext : DbContext
        {
            public AppDbContext() : base(GetConnectionString()) { }

            public DbSet<Clients> Clients { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<Users> Users { get; set; }

            private static string GetConnectionString()
            {
                string dbPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "orders.db");
                return $"Data Source={dbPath};Version=3;";
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                // Настройка связей
                modelBuilder.Entity<Order>()
                    .HasRequired(o => o.Clients)
                    .WithMany()
                    .HasForeignKey(o => o.id_Client);

                modelBuilder.Entity<Order>()
                    .HasRequired(o => o.Product)
                    .WithMany()
                    .HasForeignKey(o => o.ProductArticle);

                modelBuilder.Entity<Order>()
                    .HasRequired(o => o.Users)
                    .WithMany()
                    .HasForeignKey(o => o.ManagerId);
            }
        }

    }
}