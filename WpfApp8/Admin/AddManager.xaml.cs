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

namespace WpfApp8.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddManager.xaml
    /// </summary>
    public partial class AddManager : Window
    {
        public Users Users { get; set; }
        public bool IsNewUsers { get; set; }

        public AddManager()
        {
            InitializeComponent();
            Users = new Users();
            IsNewUsers = true;
        }

        public AddManager(Users existingUsers)
        {
            InitializeComponent();
            Users = existingUsers;
            IsNewUsers = false;

            // Заполняем поля существующими данными
            idTextBox.Text = Users.id.ToString();
            NameTextBox.Text = Users.Name;
            SurnameTextBox.Text = Users.Surname.ToString();
            PatronymicTextBox.Text = Users.Patronymic;
            BirthDateTextBox.Text = Users.BirthDate.ToString();
            LoginTextBox.Text = Users.Login;
            PasswordTextBox.Text = Users.Password;
            PhoneTextBox.Text = Users.Phone;
            AdressTextBox.Text = Users.Adress;
            id_RoleTextBox.Text = Users.id_Role.ToString();
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
                string.IsNullOrEmpty(id_RoleTextBox.Text))

            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (IsNewUsers)
                {
                    // Для нового продукта
                    Users.id = int.Parse(idTextBox.Text);
                    Users.Name = NameTextBox.Text;
                    Users.Surname = SurnameTextBox.Text;
                    Users.Patronymic = PatronymicTextBox.Text;
                    Users.BirthDate = DateTime.Parse(BirthDateTextBox.Text);
                    Users.Login = LoginTextBox.Text;
                    Users.Password = PasswordTextBox.Text;
                    Users.Phone = PhoneTextBox.Text;
                    Users.Adress = AdressTextBox.Text;
                    Users.id_Role = int.Parse(id_RoleTextBox.Text);
                }
                else
                {
                    // Для существующего продукта
                    Users.id = int.Parse(idTextBox.Text);
                    Users.Name = NameTextBox.Text;
                    Users.Surname = SurnameTextBox.Text;
                    Users.Patronymic = PatronymicTextBox.Text;
                    Users.BirthDate = DateTime.Parse(BirthDateTextBox.Text);
                    Users.Login = LoginTextBox.Text;
                    Users.Password = PasswordTextBox.Text;
                    Users.Phone = PhoneTextBox.Text;
                    Users.Adress = AdressTextBox.Text;
                    Users.id_Role = int.Parse(id_RoleTextBox.Text);
                }

                DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка формата данных. Проверьте поля 'id', 'Роль' и 'Дата рождения'",
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
