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

namespace PetShop321.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrEmpty(LoginTextBox.Text))
                {
                    errors.AppendLine("Заполните логин");
                }
                if (string.IsNullOrEmpty(PasswordBox.Password))
                {
                    errors.AppendLine("Заполните пароль");
                }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (Data.Trade2Entities.GetContext().User
                    .Any(d => d.UserLogin == LoginTextBox.Text
                    && d.UserPassword == PasswordBox.Password))
                {
                    var user = Data.Trade2Entities.GetContext().User
                        .Where(d => d.UserLogin == LoginTextBox.Text
                        && d.UserPassword == PasswordBox.Password).FirstOrDefault();
                    switch (user.Role.RoleName)
                    {
                        case "Администратор":
                            Classes.Manager.MainFrame.Navigate(new Pages.ViewProductPage());
                            break;
                        case "Клиент":
                            Classes.Manager.MainFrame.Navigate(new Pages.ViewProductPage());
                            break;
                        case "Менеджер":
                            Classes.Manager.MainFrame.Navigate(new Pages.ViewProductPage());
                            break;
                    }
                    MessageBox.Show("Успех", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Учетной записи не найдено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
