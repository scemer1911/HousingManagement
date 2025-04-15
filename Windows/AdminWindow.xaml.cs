using System.Windows;
using HousingManagement.Windows;

namespace HousingManagement.Windows
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ManageClients_Click(object sender, RoutedEventArgs e)
        {
            var window = new ResidentWindow(); // окно для работы с жильцами
            window.ShowDialog();
        }

        private void ManageServices_Click(object sender, RoutedEventArgs e)
        {
            var window = new ServiceWindow(); // окно для работы с услугами
            window.ShowDialog();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
