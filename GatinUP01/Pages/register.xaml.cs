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

using Dapper;

namespace GatinUP01.Pages
{
    /// <summary>
    /// Логика взаимодействия для auth.xaml
    /// </summary>
    public partial class register : Page
    {
        public register()
        {
            InitializeComponent();
        }

        private void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            string newName = Input_Name.Text;
            string newLogin = Input_Login.Text;
            string newPassword = Input_Password.Text;

            bool isCorrect = 
                CORE.Check_CorrectNumbers(newName, 4, "Введите минимум 4 символов в указании имени",12, "Введите максимум 12 символов в указании имени") & 
                CORE.Check_CorrectNumbers(newLogin, 4, "Введите минимум 4 символов в указании логина",12, "Введите максимум 12 символов в указании логина") &
                CORE.Check_CorrectNumbers(newPassword, 4, "Введите минимум 4 символов в указании пароля", 12, "Введите максимум 12 символов в указании пароля");

            if(!isCorrect)
            {
                return;
            }



            using (var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var sql = "SELECT * FROM [User] WHERE Name = @Name OR Login = @Login";
                var currentUser = connection.QueryFirstOrDefault<User>(sql, new { Name = Input_Name.Text, Login = Input_Login.Text});
                if(currentUser != null) {
                    MessageBox.Show("Такой пользователь уже зарегистрирован");
                    return;
                }

                var sql2 = "INSERT INTO [User] (Name, Login, Password, RoleID) VALUES (@Name, @Login, @Password, @RoleID)";
                User newUser = new User(newName, newLogin, CORE.GetHash(newPassword), 0);
                int newRow = connection.Execute(sql2, newUser);
                if (newRow > 0) {
                    MessageBox.Show("Пользователь зарегистрирован");
                    NavigationService.Navigate(new main(newUser));
                }else{
                    MessageBox.Show("Не удалось добавить пользователя");
                }
            }
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth());
        }
    }
}
