using System.Linq;
using System.Windows;
using HousingManagement;
using HousingManagement.Helpers;
using System.Data.Entity;

namespace HousingManagement.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string passwordHash = PasswordHelper.ComputeHash(password);

            using (var db = new HousingDBEntities())
            {
                // Используем Include для загрузки связанных данных о ролях
                var user = db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Login == login);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с таким логином не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.PasswordHash != passwordHash)
                {
                    MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string roleName = user.Roles?.Name;

                if (roleName == null)
                {
                    MessageBox.Show("Роль пользователя не определена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show($"Успешный вход как {roleName}");

                Window nextWindow;

                if (roleName.ToLower() == "администратор")
                    nextWindow = new AdminWindow();
                else if (roleName.ToLower() == "жилец")
                    nextWindow = new UserWindow();
                else
                {
                    MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                nextWindow.Show();
                this.Close();
            }
        }
    }
}
