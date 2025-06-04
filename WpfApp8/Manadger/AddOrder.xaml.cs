using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfApp8.Entities;

namespace WpfApp8.Manadger
{
    public partial class AddOrder : Window
    {
        public Order Order { get; set; }
        public bool IsNewOrder { get; set; }

        public AddOrder()
        {
            InitializeComponent();
            Order = new Order();
            IsNewOrder = true;
            LoadManagers();
            LoadStatuses(); // Добавьте эту строку
        }

        public AddOrder(Order existingOrder)
        {
            InitializeComponent();
            Order = existingOrder;
            IsNewOrder = false;
            LoadManagers();
            LoadStatuses(); // Добавьте эту строку

            // Заполняем поля существующими данными
            CodeTextBox.Text = Order.Code.ToString();
            StatusComboBox.Text = Order.Status;
            DatePicker.Text = Order.Date.ToString();
            id_ClientTextBox.Text = Order.id_Client.ToString();
            ProductArticleTextBox.Text = Order.ProductArticle;
            QuantityTextBox.Text = Order.Quantity;

            // Устанавливаем выбранного менеджера
            if (Order.ManagerId.HasValue)
            {
                ManagerComboBox.SelectedValue = Order.ManagerId.Value;
            }
        }

        private void LoadManagers()
        {
            try
            {
                using (var db = new diplomchikEntities())
                {
                    // Сначала загружаем данные как есть
                    var managers = db.Users
                        .Where(u => u.id_Role == 1) // Фильтр по роли "Менеджер"
                        .ToList(); // Материализуем запрос

                    // Затем преобразуем для отображения ФИО уже в памяти
                    var managerList = managers.Select(u => new
                    {
                        u.id,
                        FullName = $"{u.Surname} {u.Name} {u.Patronymic}"
                    }).ToList();

                    ManagerComboBox.ItemsSource = managerList;
                    ManagerComboBox.DisplayMemberPath = "FullName";
                    ManagerComboBox.SelectedValuePath = "id";

                    // Для нового заказа выбираем первого менеджера по умолчанию
                    if (IsNewOrder && managerList.Any())
                    {
                        ManagerComboBox.SelectedIndex = 0;
                    }

                    // Для существующего заказа устанавливаем выбранного менеджера
                    if (!IsNewOrder && Order.ManagerId.HasValue)
                    {
                        ManagerComboBox.SelectedValue = Order.ManagerId.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки менеджеров: {ex.Message}");
            }
        }
        private void LoadStatuses()
        {
            // Здесь можно указать возможные статусы заказов
            var statuses = new List<string>
            {
                "Активный",
                "В обработке",
                "Завершен",
                "Отменен"
            };

            StatusComboBox.ItemsSource = statuses;

            // Если редактируем существующий заказ, устанавливаем текущий статус
            if (!IsNewOrder)
            {
                StatusComboBox.SelectedItem = Order.Status;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrEmpty(CodeTextBox.Text) ||
                string.IsNullOrEmpty(StatusComboBox.Text) ||
                string.IsNullOrEmpty(DatePicker.Text) ||
                string.IsNullOrEmpty(id_ClientTextBox.Text) ||
                string.IsNullOrEmpty(ProductArticleTextBox.Text) ||
                string.IsNullOrEmpty(QuantityTextBox.Text) ||
                ManagerComboBox.SelectedValue == null) // Проверка выбора менеджера
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Order.Code = CodeTextBox.Text;
                Order.Status = StatusComboBox.Text;
                Order.Date = DateTime.Parse(DatePicker.Text);
                Order.id_Client = int.Parse(id_ClientTextBox.Text);
                Order.ProductArticle = ProductArticleTextBox.Text;
                Order.Quantity = QuantityTextBox.Text;
                Order.ManagerId = (int)ManagerComboBox.SelectedValue; // Сохраняем ID менеджера

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