using System.Linq;
using System.Windows;
using HousingManagement;
using System.Collections.Generic;
using System;

namespace HousingManagement.Windows
{
    public partial class ServiceWindow : Window
    {
        private readonly HousingDBEntities _db;
        private readonly List<ResidentService> _selectedServices;

        public ServiceWindow()
        {
            InitializeComponent();
            _db = new HousingDBEntities();
            _selectedServices = new List<ResidentService>();

            // Загрузка всех жильцов из базы данных и привязка их к ComboBox
            ResidentsComboBox.ItemsSource = _db.Residents.ToList();

            // Загрузка доступных услуг из базы данных и привязка к ComboBox
            ServicesComboBox.ItemsSource = _db.Services.ToList();
        }

        // Обработчик клика по кнопке "Добавить услугу"
        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesComboBox.SelectedItem != null && int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
            {
                var service = (Services)ServicesComboBox.SelectedItem;

                // Добавляем выбранную услугу в список
                _selectedServices.Add(new ResidentService
                {
                    ServiceId = service.Id,
                    Quantity = quantity,
                    Service = service
                });

                // Обновляем ListView с выбранными услугами
                InvoiceListView.ItemsSource = null;
                InvoiceListView.ItemsSource = _selectedServices;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу и укажите количество.");
            }
        }

        // Обработчик клика по кнопке "Создать счет"
        private void CreateInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedServices.Count == 0)
            {
                MessageBox.Show("Не выбраны услуги для составления счета.");
                return;
            }

            // Проверяем, выбран ли жилец
            var selectedResident = ResidentsComboBox.SelectedItem as Residents;
            if (selectedResident == null)
            {
                MessageBox.Show("Пожалуйста, выберите жильца.");
                return;
            }

            // Создание нового счета
            var invoice = new Invoices
            {
                ResidentId = selectedResident.Id,
                InvoiceDate = DateTime.Now
                // TotalAmount удалён, так как его нет в БД
            };

            _db.Invoices.Add(invoice);
            _db.SaveChanges();

            // Привязываем услуги к новому счету
            foreach (var selectedService in _selectedServices)
            {
                _db.ResidentServices.Add(new ResidentServices
                {
                    ResidentId = selectedResident.Id,
                    ServiceId = selectedService.ServiceId,
                    IsFixed = false,
                    CustomRate = selectedService.Service.Rate
                });
            }

            _db.SaveChanges();

            MessageBox.Show("Счет успешно создан!");
            this.Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class ResidentService
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public Services Service { get; set; }
    }
}
