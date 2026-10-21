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
    /// Логика взаимодействия для profile.xaml
    /// </summary>
    public partial class profile : Page
    {
        private User user;
        private string cptch;
        public profile(User currentUser)
        {
            InitializeComponent();
            user = currentUser;
            Label_Name.Content = $"Имя: {user.Name}";
            Label_Login.Content = $"Логин: {user.Login}";
            Label_Role.Content = $"Роль: {CORE.GetUserRole(currentUser)}";
            Input_NewPassword.Text = "";
            Input_ConfirmPassword.Text = "";
            CreateNewCaptcha();
        }

        private void CreateNewCaptcha()
        {
            cptch = CORE.GenerateCaptcha();
            Label_Captcha.Content = cptch;
            Input_Captcha.Text = "";
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new main(user));
        }

        private void Button_ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = Input_NewPassword.Text;
            string confirmPassword = Input_ConfirmPassword.Text;
            string captcha = Input_Captcha.Text;

            bool isCorrect = CORE.Check_CorrectNumbers(newPassword, 4, "Введите минимум 4 символов в указании пароля", 12, "Введите максимум 12 символов в указании пароля");
            if (!isCorrect)
                return;
            
            if(newPassword != confirmPassword) {
                MessageBox.Show("Подтвержденный пароль не соотноситься с новым паролем. Проверьте подтвержденный пароль.");
            }

            if(captcha != cptch)
            {
                MessageBox.Show("Введите правильную капчу");
                CreateNewCaptcha();
                return;
            }

            string hashPassword = CORE.GetHash(newPassword);
            string sql = "UPDATE [User] SET Password = @Password WHERE ID = @ID";
            using (var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var foundUser = connection.Execute(sql, new { Password = hashPassword, ID = user.ID });
                if(foundUser > 0)
                {
                    user.Password = hashPassword;
                    MessageBox.Show("Пароль изменён");
                }
            }
        }
    }
}
