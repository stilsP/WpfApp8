using System;
using System.Linq;
using System.Windows;
using WpfApp8.Entities;

namespace WpfApp8.Admin
{
    public partial class AddManager : Window
    {
        public Users Users { get; set; }
        public bool IsNewUsers { get; set; }

        public AddManager()
        {
            InitializeComponent();
            Users = new Users();
            IsNewUsers = true;
            LoadRoles();
        }

        public AddManager(Users existingUsers)
        {
            InitializeComponent();
            Users = existingUsers;
            IsNewUsers = false;
            LoadRoles();

            // Заполняем поля существующими данными
            idTextBox.Text = Users.id.ToString();
            NameTextBox.Text = Users.Name;
            SurnameTextBox.Text = Users.Surname;
            PatronymicTextBox.Text = Users.Patronymic;
            BirthDateTextBox.Text = Users.BirthDate.ToString();
            LoginTextBox.Text = Users.Login;
            PasswordTextBox.Text = Users.Password;
            PhoneTextBox.Text = Users.Phone;
            AdressTextBox.Text = Users.Adress;

            // Устанавливаем выбранную роль
            if (Users.id_Role > 0)
            {
                RoleComboBox.SelectedValue = Users.id_Role;
            }
        }

        private void LoadRoles()
        {
            try
            {
                using (var db = new diplomchikEntities())
                {
                    RoleComboBox.ItemsSource = db.Role.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrEmpty(idTextBox.Text) ||
                string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(SurnameTextBox.Text) ||
                string.IsNullOrEmpty(PatronymicTextBox.Text) ||
                string.IsNullOrEmpty(BirthDateTextBox.Text) ||
                string.IsNullOrEmpty(LoginTextBox.Text) ||
                string.IsNullOrEmpty(PasswordTextBox.Text) ||
                string.IsNullOrEmpty(PhoneTextBox.Text) ||
                string.IsNullOrEmpty(AdressTextBox.Text) ||
                RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Users.id = int.Parse(idTextBox.Text);
                Users.Name = NameTextBox.Text;
                Users.Surname = SurnameTextBox.Text;
                Users.Patronymic = PatronymicTextBox.Text;
                Users.BirthDate = DateTime.Parse(BirthDateTextBox.Text);
                Users.Login = LoginTextBox.Text;
                Users.Password = PasswordTextBox.Text;
                Users.Phone = PhoneTextBox.Text;
                Users.Adress = AdressTextBox.Text;
                Users.id_Role = (int)RoleComboBox.SelectedValue;

                DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка формата данных. Проверьте поля 'id' и 'Дата рождения'",
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