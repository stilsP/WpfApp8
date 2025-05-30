using System;
using System.Collections.Generic;
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
using static WpfApp8.MainWindow;
using static WpfApp8.MainWindow_User;

namespace WpfApp8.Manadger
{
    /// <summary>
    /// Логика взаимодействия для CustomerOrdersWindow.xaml
    /// </summary>
    public partial class CustomerOrdersWindow : Window
    {
        private readonly int _clientId;

        public CustomerOrdersWindow(int clientId)
        {
            InitializeComponent();
            _clientId = clientId;
            LoadOrders();
        }

        private void LoadOrders()
        {
            using (var db = new AppDbContext())
            {
                // Для явной загрузки связанных данных используем Load()
                db.Configuration.LazyLoadingEnabled = false;

                var orders = db.Orders
                    .Where(o => o.id_Client == _clientId)
                    .OrderByDescending(o => o.Date)
                    .ToList();

                // Явная загрузка связанных продуктов
                foreach (var order in orders)
                {
                    db.Entry(order).Reference(o => o.Product).Load();
                }

                OrdersListView.ItemsSource = orders;
            }
        }
    }
}
