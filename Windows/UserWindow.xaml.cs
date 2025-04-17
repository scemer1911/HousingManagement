using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HousingManagement.Windows
{
    public partial class UserWindow : Window
    {
        private readonly int _residentId;

        public UserWindow(int residentId)
        {
            InitializeComponent();
            _residentId = residentId;
            LoadInvoices();
        }

        private void LoadInvoices()
        {
            using (var db = new HousingDBEntities())
            {
                var invoices = db.Invoices
                    .Where(i => i.ResidentId == _residentId)
                    .ToList()
                    .Select(i => new InvoiceViewModel
                    {
                        Id = i.Id,
                        InvoiceDate = i.InvoiceDate ?? DateTime.MinValue,
                        Amount = i.Amount ?? 0,
                        IsPaid = i.IsPaid ?? false,
                        Services = i.ResidentServices.Select(rs => rs.Services.Name).ToList()
                    })
                    .ToList();

                InvoicesDataGrid.ItemsSource = invoices;
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PayInvoice_Click(object sender, RoutedEventArgs e)
        {
            var selectedInvoice = InvoicesDataGrid.SelectedItem as InvoiceViewModel;

            if (selectedInvoice == null)
            {
                MessageBox.Show("Выберите счёт для оплаты.");
                return;
            }

            using (var db = new HousingDBEntities())
            {
                var invoice = db.Invoices.FirstOrDefault(i => i.Id == selectedInvoice.Id);

                if (invoice != null && !(invoice.IsPaid ?? false))
                {
                    invoice.IsPaid = true;
                    db.SaveChanges();
                    MessageBox.Show("Счёт оплачен.");
                    LoadInvoices();
                }
                else
                {
                    MessageBox.Show("Счёт уже оплачен или не найден.");
                }
            }
        }
    }

    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public List<string> Services { get; set; }

        public string ServicesText => Services != null && Services.Any()
            ? string.Join(", ", Services)
            : "Нет услуг";
    }
}