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
            using (var context = new diplomEntities2())
            {
                try
                {
                    string login = txtLogin.Text;
                    string password = txtPassword.Password;

                    var user = context.Users.FirstOrDefault(u =>
                        u.Login == login && u.Password == password);

                    if (user != null)
                    {
                        // Создаем и показываем новое окно как модальное
                        MainWindow newWindow = new MainWindow();
                        newWindow.Show();
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