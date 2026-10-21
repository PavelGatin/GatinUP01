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
using System.Windows.Shapes;
using GatinUP01.Pages;
using Dapper;
using System.Data.SqlClient;

namespace GatinUP01.Windows
{
    /// <summary>
    /// Логика взаимодействия для infoDevice.xaml
    /// </summary>
    public partial class infoDevice : Window
    {
        private Device device;
        private main mainWin;
        private User user;
        public infoDevice(Device currentDevice, main mainWin_local, User user)
        {
            InitializeComponent();
            Label_Type.Content = $"Название: {TestFoundConnectingDevicesSensors.GiveNameInByID(currentDevice.DeviceID,"DeviceType")}";
            Label_Name.Content = $"Имя: {currentDevice.Name}";
            Label_Place.Content = $"Место: {currentDevice.Place}";
            TextBox_Status.Text = currentDevice.Info;

            device = currentDevice;
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

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить устройство?", "Удаление устройства", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var connection = new SqlConnection(CORE.GetConnectionString()))
                {
                    var sql = "DELETE FROM [Device] WHERE ID = @Id";
                    var connect = connection.Execute(sql, new { Id = device.ID });
                    if (connect <= 0)
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
