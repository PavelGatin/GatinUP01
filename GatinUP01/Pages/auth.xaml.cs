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
    public partial class auth : Page
    {
        private int mistakes = 0;
        private string cptch;
        private bool isCptch = false;
        public auth()
        {
            InitializeComponent();
            CptchName.IsEnabled = false;
            CaptchaField.IsEnabled = false;
            Cptch.IsEnabled = false;
        }

        private void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqlConnection(CORE.GetConnectionString()))
            {
                var sql = "SELECT * FROM [User] WHERE Login = @Login AND Password = @Password";
                var currentUser = connection.QueryFirstOrDefault<User>(sql, new { Login = Input_Login.Text, Password = CORE.GetHash(Input_Password.Text) });

                if (currentUser != null)
                {
                    if(isCptch)
                    {
                        if(CaptchaField.Text == cptch)
                        {
                            NavigationService.Navigate(new main(currentUser));
                        }
                        else
                        {
                            MessageBox.Show("Введите правильную капчу");
                            EnableCaptcha();
                        }
                    }
                    else
                    {
                        NavigationService.Navigate(new main(currentUser));
                    }
                }
                else
                {
                    MessageBox.Show("Вы указали некорректный логин или пароль");
                    mistakes++;
                    if(mistakes >= 3)
                    {
                        EnableCaptcha();
                    }
                }
            }
        }

        private void EnableCaptcha()
        {
            CptchName.IsEnabled = true;
            CaptchaField.IsEnabled = true;
            Cptch.IsEnabled = true;
            CaptchaField.Text = "";
            cptch = CORE.GenerateCaptcha();
            Cptch.Content = cptch;
            isCptch = true;
        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new register());
        }
    }
}
