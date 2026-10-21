using Dapper;
using GatinUP01.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            if (user.RoleID == 1)
            {
                Button_AddDevice.IsEnabled = true;
            }
            else
            {
                Button_AddDevice.IsEnabled = false;
            }



            UpdatePanels();
        }

        public void UpdatePanels()
        {
            List<DeviceModel> deviceModels = new List<DeviceModel>();
            List<SensorModel> sensorModels = new List<SensorModel>();

            using (var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var sql_devices = "SELECT * FROM [Device]";
                var foundDevices = connection.Query(sql_devices, null).ToList();

                for (int i = 0; i < foundDevices.Count; i++)
                {
                    deviceModels.Add(new DeviceModel
                    {
                        Id = foundDevices[i].ID,
                        Name = foundDevices[i].Name,
                        Place = foundDevices[i].Place,
                        Status = foundDevices[i].Status
                    });
                }

                var sql_sensors = "SELECT * FROM [Sensor]";
                var foundSensors = connection.Query(sql_sensors, null).ToList();

                for (int i = 0; i < foundSensors.Count; i++)
                {
                    sensorModels.Add(new SensorModel
                    {
                        Id = foundSensors[i].ID,
                        Name = foundSensors[i].Name,
                        Place = foundSensors[i].Place,
                        Status = foundSensors[i].Status
                    });
                }
            }

            DevicePanel.ItemsSource = deviceModels;
            SensorPanel.ItemsSource = sensorModels;
        }

        private Device GetDeviceByModel(DeviceModel localModel)
        {
            using (var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var sql_sensors = "SELECT * FROM [Device] WHERE ID = @ID";
                var foundDevices = connection.QueryFirstOrDefault<Device>(sql_sensors, new { ID = localModel.Id });
                return foundDevices;
            }
        }

        private Sensor GetSensorByModel(SensorModel localModel)
        {
            using (var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var sql_sensors = "SELECT * FROM [Sensor] WHERE ID = @ID";
                var foundSensors = connection.QueryFirstOrDefault<Sensor>(sql_sensors, new {ID = localModel.Id});
                return foundSensors;
            }
        }

        private void Button_AddDevice_Click(object sender, RoutedEventArgs e)
        {
            addDevice window_addDevice = new addDevice(this);
            window_addDevice.Show();
        }

        private void OpenSensor(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var clickedItem = button.DataContext as SensorModel;
            infoSensor sensor = new infoSensor(GetSensorByModel(clickedItem), this, user);
            sensor.Show();
        }

        private string UpdateNewStatusForDevice(string stat)
        {
            switch(stat)
            {
                case "Не активен": return "Активен";
                case "Активен": return "Не активен";
            }
            return "Неизвестно";
        }

        private void ActivateDevice(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var clickedItem = button.DataContext as DeviceModel;

            using(var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                string sql = "UPDATE [Device] SET Status = @Status WHERE ID = @ID";
                var connect = connection.Execute(sql, new {Status = UpdateNewStatusForDevice(clickedItem.Status), ID = clickedItem.Id});
                if(connect <= 0)
                {
                    MessageBox.Show("Ошибка при выполнении действия устройством");
                }
                else
                {
                    UpdatePanels();
                }
            }
        }

        private void InfoDevice(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var clickedItem = button.DataContext as DeviceModel;

            infoDevice device = new infoDevice(GetDeviceByModel(clickedItem), this, user);
            device.Show();
        }

        private void Button_Profile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new profile(user));
        }
    }

    public class ItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; } // Сюда можно передать URL из интернета или локальный путь (например, "/Images/photo.png")
    }

    public class DeviceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
    }

    public class SensorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
    }
}
