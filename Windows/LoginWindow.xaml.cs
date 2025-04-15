using System.Linq;
using System.Windows;
using HousingManagement.Helpers;

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
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == passwordHash);
                if (user != null)
                {
                    MessageBox.Show($"Успешный вход как {user.Role}");

                    Window nextWindow;

                    if (user.Role == "admin")
                        nextWindow = new AdminWindow();
                    else
                        nextWindow = new UserWindow();

                    nextWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
