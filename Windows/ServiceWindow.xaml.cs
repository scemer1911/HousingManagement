using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

namespace HousingManagement.Windows
{
    public partial class ServiceWindow : Window
    {
        private readonly HousingDBEntities _db;
        private readonly ObservableCollection<SelectedService> _selectedServices;

        public ServiceWindow()
        {
            InitializeComponent();
            _db = new HousingDBEntities();
            _selectedServices = new ObservableCollection<SelectedService>();

            ResidentsComboBox.ItemsSource = _db.Residents.ToList();
            ServicesComboBox.ItemsSource = _db.Services.ToList();
            InvoiceListView.ItemsSource = _selectedServices;
        }

        // Вспомогательный класс для отображения выбранных услуг
        private class SelectedService
        {
            public int ServiceId { get; set; }
            public string ServiceName { get; set; }
            public decimal Rate { get; set; }
            public int Quantity { get; set; }
            public decimal Total => Rate * Quantity;
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(ResidentsComboBox.SelectedItem is Residents resident))
            {
                MessageBox.Show("Выберите, пожалуйста, жильца.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!(ServicesComboBox.SelectedItem is Services svc) ||
                !int.TryParse(QuantityTextBox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Выберите услугу и укажите корректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedServices.Add(new SelectedService
            {
                ServiceId = svc.Id,
                ServiceName = svc.Name,
                Rate = svc.Rate ?? 0,
                Quantity = qty
            });
        }

        private void CreateInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedServices.Count == 0)
            {
                MessageBox.Show("Не добавлены услуги для счета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!(ResidentsComboBox.SelectedItem is Residents resident))
            {
                MessageBox.Show("Выберите жильца.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Вычисляем общую сумму
            decimal totalAmount = _selectedServices.Sum(x => x.Total);

            // Создаем счет
            var invoice = new Invoices
            {
                ResidentId = resident.Id,
                InvoiceDate = DateTime.Now,
                Amount = totalAmount,
                IsPaid = false
            };
            _db.Invoices.Add(invoice);
            _db.SaveChanges(); // чтобы получить invoice.Id

            // Привязываем услуги
            foreach (var sel in _selectedServices)
            {
                _db.ResidentServices.Add(new ResidentServices
                {
                    ResidentId = resident.Id,
                    ServiceId = sel.ServiceId,
                    Quantity = sel.Quantity,
                    CustomRate = sel.Rate,
                    IsFixed = false,
                    InvoiceId = invoice.Id
                });
            }
            _db.SaveChanges();

            MessageBox.Show($"Счет №{invoice.Id} создан. Сумма: {totalAmount:N2}", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}