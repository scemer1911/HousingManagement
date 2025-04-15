using System.Linq;
using System.Windows;
using HousingManagement;

namespace HousingManagement.Windows
{
    public partial class ResidentWindow : Window
    {
        private HousingDBEntities _context = new HousingDBEntities();

        public ResidentWindow()
        {
            InitializeComponent();
            LoadResidents();
        }

        // Загрузка всех жильцов
        private void LoadResidents()
        {
            ClientDataGrid.ItemsSource = _context.Residents.ToList();
        }

        // Поиск по полному имени
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                var filtered = _context.Residents
                    .Where(r => r.FullName.Contains(searchText))
                    .ToList();

                ClientDataGrid.ItemsSource = filtered;
            }
        }

        // Сброс поиска
        private void ResetSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            LoadResidents();
        }

        // Добавление нового жильца
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditResidentWindow(); // Новый объект без аргументов
            if (addWindow.ShowDialog() == true)
            {
                LoadResidents(); // Обновляем таблицу
            }
        }

        // Редактирование выбранного жильца
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ClientDataGrid.SelectedItem is Residents selectedResident)
            {
                var editWindow = new AddEditResidentWindow(selectedResident); // Редактируем существующего жильца
                if (editWindow.ShowDialog() == true)
                {
                    LoadResidents();
                }
            }
            else
            {
                MessageBox.Show("Выберите жильца для редактирования.");
            }
        }

        // Удаление выбранного жильца
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ClientDataGrid.SelectedItem is Residents selectedResident)
            {
                if (MessageBox.Show("Удалить выбранного жильца?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Residents.Remove(selectedResident);
                    _context.SaveChanges();
                    LoadResidents();
                }
            }
            else
            {
                MessageBox.Show("Выберите жильца для удаления.");
            }
        }

        // Назад
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
