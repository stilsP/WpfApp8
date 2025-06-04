using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp8.Entities;

namespace WpfApp8
{
    public partial class AddEditWindow : Window
    {
        public Product Product { get; set; }
        public bool IsNewProduct { get; set; }

        public AddEditWindow()
        {
            InitializeComponent();
            Product = new Product();
            IsNewProduct = true;
            LoadCategories();
            LoadMeasurements();
        }

        public AddEditWindow(Product existingProduct)
        {
            InitializeComponent();
            Product = existingProduct;
            IsNewProduct = false;
            LoadCategories();
            LoadMeasurements();

            // Заполняем поля существующими данными
            ArticleTextBox.Text = Product.ProductArticle;
            NameTextBox.Text = Product.Name;
            DescriptionTextBox.Text = Product.Description;
            QuantityTextBox.Text = Product.QuantityInStock.ToString();
            CostTextBox.Text = Product.Cost.ToString("N2");

            // Устанавливаем выбранную категорию
            if (Product.id_Category > 0)
            {
                CategoryComboBox.SelectedValue = Product.id_Category;
            }

            // Устанавливаем выбранную единицу измерения
            if (!string.IsNullOrEmpty(Product.Measurement))
            {
                MeasurementComboBox.SelectedItem = Product.Measurement;
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (var db = new diplomchikEntities())
                {
                    CategoryComboBox.ItemsSource = db.Category.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            }
        }

        private void LoadMeasurements()
        {
            // Стандартные единицы измерения
            var measurements = new List<string>
            {
                "шт", "упак", "набор"
            };
            MeasurementComboBox.ItemsSource = measurements;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrEmpty(ArticleTextBox.Text) ||
                string.IsNullOrEmpty(NameTextBox.Text) ||
                CategoryComboBox.SelectedValue == null ||
                string.IsNullOrEmpty(QuantityTextBox.Text) ||
                MeasurementComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(CostTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Product.ProductArticle = ArticleTextBox.Text;
                Product.Name = NameTextBox.Text;
                Product.id_Category = (int)CategoryComboBox.SelectedValue;
                Product.Description = DescriptionTextBox.Text;
                Product.QuantityInStock = int.Parse(QuantityTextBox.Text);
                Product.Measurement = MeasurementComboBox.SelectedItem.ToString();
                Product.Cost = decimal.Parse(CostTextBox.Text);

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
            // Обработчик изменения текста (если нужен)
        }
    }
}