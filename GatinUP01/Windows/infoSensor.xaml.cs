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
using Dapper;
using GatinUP01.Pages;

namespace GatinUP01.Windows
{
    /// <summary>
    /// Логика взаимодействия для infoSensor.xaml
    /// </summary>
    public partial class infoSensor : Window
    {
        private Sensor sensor;
        private main mainWin;
        private User user;
        public infoSensor(Sensor currentSensor, main mainWin_local, User user)
        {
            InitializeComponent();
            Label_Type.Content = $"Название: {TestFoundConnectingDevicesSensors.GiveNameInByID(currentSensor.SensorID, "SensorType")}";
            Label_Name.Content = $"Имя: {currentSensor.Name}";
            Label_Place.Content = $"Место: {GetPlace(currentSensor.Place)}";
            TextBox_Status.Text = currentSensor.Info;

            sensor = currentSensor;
            mainWin = mainWin_local;
            this.user = user;

            if (user.RoleID == 0)
            {
                Button_Delete.IsEnabled = false;
            }
            else
            {
                Button_Delete.IsEnabled = true;
            }
        }

        private string GetPlace(string place)
        {
            if(place.ToCharArray().Length > 0)
            {
                return place;
            }
            return "Не указано";
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить устройство?", "Удаление устройства", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using(var connection = new SqlConnection(CORE.GetConnectionString()))
                {
                    var sql = "DELETE FROM [Sensor] WHERE ID = @Id";
                    var connect = connection.Execute(sql, new { Id = sensor.ID });
                    if(connect <= 0)
                    {
                        MessageBox.Show("Не удалось удалить устройство");
                    }
                }
                mainWin.UpdatePanels();
                Close();
            }
            else
            {
                return;
            }
        }
    }
}
