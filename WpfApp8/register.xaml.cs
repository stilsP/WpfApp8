using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using System.Configuration;
using WpfApp8.Entities;
using WpfApp8.Manadger; 


namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для register.xaml
    /// </summary>
    public partial class register : Window
    {


        public register()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new diplomchikEntities())
            {
                try
                {
                    string login = txtLogin.Text;
                    string password = txtPassword.Password;

                    var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                    if (user != null)
                    {
                        // Проверьте, что поля существуют в сущности Users
                        UserSession.CurrentUserName = $"{user.Name} {user.Surname} {user.Patronymic}"; // Пример для полей ФИО
                        UserSession.CurrentUserId = user.id;

                        int? idRole = user.id_Role;

                        if (idRole == 1) // Пользователь
                        {
                            new MainWindow_User().Show();
                        }
                        else if (idRole == 2) // Администратор
                        {
                            new MainWindow().Show();
                        }
                        else
                        {
                            MessageBox.Show("Неизвестная роль.");
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}