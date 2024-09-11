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
    /// Логика взаимодействия для ViewProductPage.xaml
    /// </summary>
    public partial class ViewProductPage : Page
    {
        public ViewProductPage()
        {
            InitializeComponent();

            Init();
        }
        public void Init()
        {
            ProductListView.ItemsSource = Data.Trade2Entities.GetContext().Product.ToList();
            CountOfLabel.Content = $"{Data.Trade2Entities.GetContext().Product.Count()}";
            if (Classes.Manager.CurrentUser != null)
            {
                FIOLabel.Content = $"{Classes.Manager.CurrentUser.UserSurname}" +
                    $"{Classes.Manager.CurrentUser.UserName}" +
                    $"{Classes.Manager.CurrentUser.UserPatronymic}";
            }
            SeachTextBox.Text = string.Empty;
            SortUpRadioButton.IsChecked = false;
            SortDownRadioButton.IsChecked = false;

            var manufactList = Data.Trade2Entities.GetContext().Manufacturer.ToList();
            manufactList.Insert(0, new Data.Manufacturer { Name = "Все производители" });
            ManufacturerComboBox.ItemsSource = manufactList;
            ManufacturerComboBox.SelectedIndex = 0;
            if(Classes.Manager.CurrentUser!=null && Classes.Manager.CurrentUser.Role.RoleName == "Администратор")
            {
                //BackButton.Visibility = Visibility.Visible;
            }
        }

        public List<Data.Product> _currentProducts = Data.Trade2Entities.GetContext().Product.ToList();
        private void SeachTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SortUpRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SortDownRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
