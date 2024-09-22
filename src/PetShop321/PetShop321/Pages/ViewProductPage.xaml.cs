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
            CountOfLabel.Content = $"{Data.Trade2Entities.GetContext().Product.Count()}" +
                $"/{Data.Trade2Entities.GetContext().Product.Count()}";
            if (Classes.Manager.CurrentUser != null)
            {
                FIOLabel.Content = $"{Classes.Manager.CurrentUser.UserSurname} " +
                    $"{Classes.Manager.CurrentUser.UserName} " +
                    $"{Classes.Manager.CurrentUser.UserPatronymic}";
            }
            SeachTextBox.Text = string.Empty;
            SortUpRadioButton.IsChecked = false;
            SortDownRadioButton.IsChecked = false;

            var manufactList = Data.Trade2Entities.GetContext().Manufacturer.ToList();
            manufactList.Insert(0, new Data.Manufacturer { Name = "Все производители" });
            ManufacturerComboBox.ItemsSource = manufactList;
            ManufacturerComboBox.SelectedIndex = 0;
            //if(Classes.Manager.CurrentUser!=null && Classes.Manager.CurrentUser.Role.RoleName == "Администратор")
            //{
            //    BackButton.Visibility = Visibility.Visible;
            //}
        }

        public List<Data.Product> _currentProducts = Data.Trade2Entities.GetContext().Product.ToList();
        public void Update()
        {
            try
            {
                _currentProducts = Data.Trade2Entities.GetContext().Product.ToList();
                _currentProducts = (from item in Data.Trade2Entities.GetContext().Product where
                                    item.ProductName.Name.ToLower().Contains(SeachTextBox.Text) ||
                                    item.ProductDescription.ToLower().Contains(SeachTextBox.Text) ||
                                    item.Manufacturer.Name.ToLower().Contains(SeachTextBox.Text) ||
                                    item.ProductCost.ToString().ToLower().Contains(SeachTextBox.Text) ||
                                    item.ProductQuantityInStock.ToString().ToLower().Contains(SeachTextBox.Text)
                                    select item).ToList();
                if(SortUpRadioButton.IsChecked == true)
                {
                    _currentProducts = _currentProducts.OrderBy(d => d.ProductCost).ToList();
                }
                if(SortDownRadioButton.IsChecked == true)
                {
                    _currentProducts = _currentProducts.OrderByDescending(d => d.ProductCost).ToList();
                }

                var selected = ManufacturerComboBox.SelectedItem as Data.Manufacturer;
                if(selected != null && selected.Name != "Все производители")
                {
                    _currentProducts = _currentProducts.Where(d => d.IdProductManufacturer == selected.Id).ToList();
                }
                CountOfLabel.Content = $"{_currentProducts.Count()}" +
                    $"/{Data.Trade2Entities.GetContext().Product.Count()}";

                ProductListView.ItemsSource = _currentProducts;
            }
            catch (Exception)
            {

            }
        }
        private void SeachTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void SortUpRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void SortDownRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var selected = (sender as Button).DataContext as Data.Product;
            //    var forDelete = Data.Trade2Entities.GetContext().OrderProduct.Where(d => d.IdProduct == selected.Id).ToList();
            //    if(forDelete.Count() > 0)
            //    {
            //        MessageBox.Show("Товар, который присутствует в заказе, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        Data.Trade2Entities.GetContext().Product.Remove(selected);
            //        Data.Trade2Entities.GetContext().SaveChanges();
            //        MessageBox.Show("Успешно!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            //        Update();
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            FIOLabel.Visibility = Visibility.Collapsed;

            //Classes.Manager.User = null;

            if (Classes.Manager.MainFrame.CanGoBack)
            {
                Classes.Manager.MainFrame.GoBack();
            }
        }
    }
}
