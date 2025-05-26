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

namespace WpfApp8.Manadger
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public Order Order { get; set; }
        public bool IsNewOrder { get; set; }

        public AddOrder()
        {
            InitializeComponent();
            Order = new Order();
            IsNewOrder = true;
        }

        public AddOrder(Order existingOrder)
        {
            InitializeComponent();
            Order = existingOrder;
            IsNewOrder = false;

            // Заполняем поля существующими данными
            CodeTextBox.Text = Order.Code.ToString();
            StatusTextBox.Text = Order.Status;
            DateTextBox.Text = Order.Date.ToString();
            id_ClientTextBox.Text = Order.id_Client.ToString();
            ProductArticleTextBox.Text = Order.ProductArticle;
            QuantityTextBox.Text = Order.Quantity;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrEmpty(CodeTextBox.Text) ||
                string.IsNullOrEmpty(StatusTextBox.Text) ||
                string.IsNullOrEmpty(DateTextBox.Text) ||
                string.IsNullOrEmpty(id_ClientTextBox.Text) ||
                string.IsNullOrEmpty(ProductArticleTextBox.Text) ||
                string.IsNullOrEmpty(QuantityTextBox.Text))

            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (IsNewOrder)
                {
                    // Для нового продукта
                    Order.Code = CodeTextBox.Text;
                    Order.Status = StatusTextBox.Text;
                    Order.Date = DateTime.Parse(DateTextBox.Text);
                    Order.id_Client = int.Parse(id_ClientTextBox.Text);
                    Order.ProductArticle = ProductArticleTextBox.Text;
                    Order.Quantity = QuantityTextBox.Text;

                }
                else
                {
                    // Для существующего продукта
                    Order.Code = CodeTextBox.Text;
                    Order.Status = StatusTextBox.Text;
                    Order.Date = DateTime.Parse(DateTextBox.Text);
                    Order.id_Client = int.Parse(id_ClientTextBox.Text);
                    Order.ProductArticle = ProductArticleTextBox.Text;
                    Order.Quantity = QuantityTextBox.Text;
                }

                DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка формата данных. Проверьте поля 'Code', 'id Клиента' и 'Дата заказа'",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
