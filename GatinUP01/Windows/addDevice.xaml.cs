using Dapper;
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
using System.Windows.Shapes;

namespace GatinUP01.Windows
{
    /// <summary>
    /// Логика взаимодействия для addDevice.xaml
    /// </summary>
    public partial class addDevice : Window
    {
        private bool isFoundDevices = false;
        private FoundElectroElements electroElement;

        public addDevice()
        {
            InitializeComponent();
            IsConnectingLabel();
            FindDevices();
        }

        private async void FindDevices()
        {
            Tab_Settings.IsEnabled = false;
            await Task.Delay(3000);
            DevicesHaveBeenFound();
        }

        private void DevicesHaveBeenFound()
        {
            isFoundDevices = true;

            List<FoundElectroElements> foundElements = TestFoundConnectingDevicesSensors.GetFoundSensorsDevices();
            Connecting_List.DisplayMemberPath = "Name";
            //Connecting_List.ValueMember = "ID";
            Connecting_List.ItemsSource = foundElements;
        }

        private async void IsConnectingLabel()
        {
            int idAnim = 0;
            while(!isFoundDevices)
            {
                await Task.Delay(250);
                idAnim++;
                UpdateConnectingLabel(idAnim);
            }

            Connecting_Label.Content = "Устройства найдены";
        }

        private void UpdateConnectingLabel(int id)
        {
            int processedId = id % 4;
            switch(processedId)
            {
                case 0: Connecting_Label.Content = "Поиск устройств"; break;
                case 1: Connecting_Label.Content = "Поиск устройств."; break;
                case 2: Connecting_Label.Content = "Поиск устройств.."; break;
                case 3: Connecting_Label.Content = "Поиск устройств..."; break;
            }
        }

        private void Button_AddDevice_Click(object sender, RoutedEventArgs e)
        {
            string device_name = Field_Name.Text;
            string device_place = Field_Place.Text;
            bool isCorrect = CORE.Check_CorrectNumbers(device_name, 1, "Введите хотя бы одну букву в название", 255, "Вы ввели слишком большое название");
            if(!isCorrect)
                return;

            using(var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var sql = "";
                int newRow = 0;
                switch (electroElement) //Определяет куда добавлять устройство и в какую таблицу базы данных
                {
                    case FoundDevices device:
                        sql = "INSERT INTO [Device] (Name, DeviceID, Place) VALUES (@Name, @DeviceID, @Place)";
                        Device newDevice = new Device(device_name, device.devicesType, device_place);
                        newRow = connection.Execute(sql, newDevice);
                        break;

                    case FoundSensors sensor:
                        sql = "INSERT INTO [Sensor] (Name, SensorID, Place) VALUES (@Name, @SensorID, @Place)";
                        Sensor newSensor = new Sensor(device_name, sensor.sensorsType, device_place);
                        newRow = connection.Execute(sql, newSensor);
                        break;
                }

                if(newRow > 0)
                {
                    MessageBox.Show("Устройство добавлено");
                }
                else
                {
                    MessageBox.Show("Не удалось добавить устройство");
                }
                this.Close();
            }
        }

        private void Connecting_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            electroElement = (FoundElectroElements)Connecting_List.SelectedItem;
            Tab_Settings.IsEnabled = true;

            switch (electroElement) //Определяет куда добавлять устройство и в какую таблицу базы данных
            {
                case FoundDevices device:
                    //Label_Type.Content = TestFoundConnectingDevicesSensors.GiveNameInByID(device.devicesType, "Device");
                    //Label_Name.Content = device.Name;
                    break;

                case FoundSensors sensor:
                    //Label_Type.Content = TestFoundConnectingDevicesSensors.GiveNameInByID(sensor.sensorsType, "Sensor");
                    //Label_Name.Content = sensor.Name;
                    break;
            }
        }
    }
}
