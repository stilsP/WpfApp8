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

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public Product Product { get; set; }
        public bool IsNewProduct { get; set; }

        public AddEditWindow()
        {
            InitializeComponent();
            Product = new Product();
            IsNewProduct = true;
        }

        public AddEditWindow(Product existingProduct)
        {
            InitializeComponent();
            Product = existingProduct;
            IsNewProduct = false;

            // Заполняем поля существующими данными
            ArticleTextBox.Text = Product.ProductArticle;
            NameTextBox.Text = Product.Name;
            CategoryComboBox.Text = Product.id_Category.ToString();
            DescriptionTextBox.Text = Product.Description;
            QuantityTextBox.Text = Product.QuantityInStock.ToString();
            MeasurementTextBox.Text = Product.Measurement;
            CostTextBox.Text = Product.Cost.ToString("N2");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrEmpty(ArticleTextBox.Text) ||
                string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(CategoryComboBox.Text) ||
                string.IsNullOrEmpty(QuantityTextBox.Text) ||
                string.IsNullOrEmpty(MeasurementTextBox.Text) ||
                string.IsNullOrEmpty(CostTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (IsNewProduct)
                {
                    // Для нового продукта
                    Product.ProductArticle = ArticleTextBox.Text;
                    Product.Name = NameTextBox.Text;
                    Product.id_Category = int.Parse(CategoryComboBox.Text);
                    Product.Description = DescriptionTextBox.Text;
                    Product.QuantityInStock = int.Parse(QuantityTextBox.Text);
                    Product.Measurement = MeasurementTextBox.Text;
                    Product.Cost = decimal.Parse(CostTextBox.Text);
                }
                else
                {
                    // Для существующего продукта
                    Product.Name = NameTextBox.Text;
                    Product.id_Category = int.Parse(CategoryComboBox.Text);
                    Product.Description = DescriptionTextBox.Text;
                    Product.QuantityInStock = int.Parse(QuantityTextBox.Text);
                    Product.Measurement = MeasurementTextBox.Text;
                    Product.Cost = decimal.Parse(CostTextBox.Text);
                }

                DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка формата данных. Проверьте поля 'Количество' и 'Цена'",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
