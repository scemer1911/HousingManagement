using System;
using System.Windows;

namespace HousingManagement.Windows
{
    public partial class CreateServiceWindow : Window
    {
        private readonly HousingDBEntities _db;

        public CreateServiceWindow()
        {
            InitializeComponent();
            _db = new HousingDBEntities();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServiceNameTextBox.Text) || !decimal.TryParse(RateTextBox.Text, out decimal rate))
            {
                MessageBox.Show("Введите корректные данные.");
                return;
            }

            var newService = new Services
            {
                Name = ServiceNameTextBox.Text.Trim(),
                Rate = rate
            };

            _db.Services.Add(newService);
            _db.SaveChanges();

            MessageBox.Show("Услуга добавлена.");
            this.DialogResult = true;
            this.Close();
        }
    }
}
