using System.Windows;
using HousingManagement;

namespace HousingManagement.Windows
{
    public partial class AddEditResidentWindow : Window
    {
        private Residents _resident;

        // Конструктор для редактирования существующего жильца
        public AddEditResidentWindow(Residents resident)
        {
            InitializeComponent();
            _resident = resident;
            // Заполняем поля для редактирования данными
            FullNameTextBox.Text = _resident.FullName;
            AddressTextBox.Text = _resident.Address;
        }

        // Конструктор для добавления нового жильца
        public AddEditResidentWindow()
        {
            InitializeComponent();
            // Инициализация для нового жильца
        }

        // Сохранение изменений или добавление нового жильца
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_resident != null)
            {
                // Обновление данных для существующего жильца
                _resident.FullName = FullNameTextBox.Text;
                _resident.Address = AddressTextBox.Text;
            }
            else
            {
                // Создание нового жильца
                var newResident = new Residents
                {
                    FullName = FullNameTextBox.Text,
                    Address = AddressTextBox.Text
                };
                // Добавление нового жильца в базу
                var context = new HousingDBEntities();
                context.Residents.Add(newResident);
                context.SaveChanges();
            }

            this.DialogResult = true; // Возвращаем результат, чтобы окно было закрыто
        }
    }
}
