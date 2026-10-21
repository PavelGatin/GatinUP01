using GatinUP01.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GatinUP01.Pages
{
    /// <summary>
    /// Логика взаимодействия для main.xaml
    /// </summary>


    public partial class main : Page
    {
        public User user;
        public main(User user)
        {
            InitializeComponent();
            Username.Content = user.Name;
            this.user = user;

            // Создаем тестовый список
            List<ItemModel> items = new List<ItemModel>()
            {
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 1", Description = "Описание первого товара в несколько строк.", ImagePath = "https://placeholder.com" },
                new ItemModel { Title = "Товар 2", Description = "Описание второго товара.", ImagePath = "https://placeholder.com" }
            };

            // Привязываем к элементу (в зависимости от выбранного варианта)

            if(user.RoleID == 1)
            {
                Button_AddDevice.IsEnabled = true;
            }
            else
            {
                Button_AddDevice.IsEnabled = false;
            }
            DevicePanel.ItemsSource = items;
            SensorPanel.ItemsSource = items;
        }

        private void Button_AddDevice_Click(object sender, RoutedEventArgs e)
        {
            addDevice window_addDevice = new addDevice();
            window_addDevice.Show();
        }
    }

    public class ItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; } // Сюда можно передать URL из интернета или локальный путь (например, "/Images/photo.png")
    }
}
