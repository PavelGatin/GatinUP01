using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace GatinUP01
{
    internal static class CORE
    {
        //private static string connectionString = @"Server=bd-kip.fa.ru;Database=GatinUP01;User Id=sa;Password=1qaz!QAZ;TrustServerCertificate=True;";
        private static string connectionString = @"Server=localhost;Database=GatinUP01;Trusted_Connection=True;TrustServerCertificate=True;";
        //private static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=GatinUP01;Trusted_Connection=True;TrustServerCertificate=True;";



        public static string GetConnectionString()
        {
            return connectionString;
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        public static string GenerateCaptcha()
        {
            string result = "";
            char[] a = { ',' };
            string arrayWords = "A,B,C,D,E,F,G,H,I" +
                "a,b,c,d,e,f,g,h,i" +
                "1,2,3,4,5,6,7,8,9,0";
            String[] words = arrayWords.Split(a);
            Random rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                result += words[rand.Next(0, words.Length)];
            }
            return result;
        }

        public static bool Check_CorrectNumbers(string input, int shortNum, string tooShort, int longNum, string tooLong)
        {
            if (input.ToCharArray().Length < shortNum)
            {
                MessageBox.Show(tooShort);
                return false;
            }
            else if (input.ToCharArray().Length > longNum)
            {
                MessageBox.Show(tooLong);
                return false;
            }
            return true;
        }
    }


    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }

        public User()
        {

        }

        public User(string name, string login, string password, int roleID)
        {
            Name = name;
            Login = login;
            Password = password;
            RoleID = roleID;
        }
    }

    public class Device
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DeviceID { get; set; }
        public string Place { get; set; }
        
        public Device(string name, int deviceID, string place)
        {
            Name = name;
            DeviceID = deviceID;
            Place = place;
        }
    }

    public class Sensor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SensorID { get; set; }
        public string Place { get; set; }

        public Sensor(string name, int sensorID, string place)
        {
            Name = name;
            SensorID = sensorID;
            Place = place;
        }
    }

}