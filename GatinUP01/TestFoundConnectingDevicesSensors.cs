using Dapper;
using GatinUP01.Pages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace GatinUP01
{
    internal static class TestFoundConnectingDevicesSensors
    {
        public static List<FoundElectroElements> GetFoundSensorsDevices()
        {
            List<FoundElectroElements> result = new List<FoundElectroElements>();

            Random rand = new Random();
            int sensorsLength = rand.Next(0, 5);
            int devicesLength = rand.Next(0, 5);

            for(int i = 0; i < sensorsLength; i++)
            {
                int sensorID = rand.Next(1, 6);
                result.Add(new FoundSensors(i, $"Sensor - {GiveNameInByID(sensorID, "SensorType")}", sensorID));
            }

            for (int i = 0; i < devicesLength; i++)
            {
                int deviceID = rand.Next(1, 4);
                result.Add(new FoundDevices(i + sensorsLength, $"Device - {GiveNameInByID(deviceID, "DeviceType")}", deviceID));
            }

            return result;
        }

        public static string GiveNameInByID(int id, string electro)
        {
            using (var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var sql = $"SELECT * FROM [{electro}] WHERE ID = @ID";
                var currentVal = connection.QueryFirstOrDefault(sql, new { ID = id});
                return currentVal.Name;
            }
        }
    }

    public class ElectroElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


    public class FoundElectroElements
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class FoundSensors : FoundElectroElements
    {
        public int sensorsType { get; set; }

        public FoundSensors(int id, string name, int sensorID)
        {
            this.ID = id;
            this.Name = name;
            this.sensorsType = sensorID;
        }

    }

    public class FoundDevices : FoundElectroElements
    {
        public int devicesType { get; set; }

        public FoundDevices(int id, string name, int deviceID)
        {
            this.ID = id;
            this.Name = name;
            this.devicesType = deviceID;
        }
    }
}
