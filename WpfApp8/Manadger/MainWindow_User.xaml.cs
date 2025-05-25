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
using WpfApp8.Entities;
using WpfApp8.Manadger;

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow_User.xaml
    /// </summary>
    public partial class MainWindow_User : Window
    {
         public MainWindow_User()
        {
            InitializeComponent();

            // Загрузка данных заказов
            LoadOrders();
        }

        private void LoadOrders()
        {
            using (var context = new diplomchikEntities())
            {
                var orders = context.Order.ToList();
                dataGridOrders.ItemsSource = orders;
            }
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            clients newWindow = new clients();
            newWindow.Show();
            this.Close();
        }

        private void CatalogButton_Click(object sender, RoutedEventArgs e)
        {
            Catalog newWindow = new Catalog();
            newWindow.Show();
            this.Close();
        }
    }
}
