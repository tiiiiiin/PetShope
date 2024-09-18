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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public string FlagAddOrEdit = "default";
        public Data.Product CurrentProduct = new Data.Product();
        public AddEditPage(Data.Product _product)
        {
            InitializeComponent();

            if(_product == null)
            {
                FlagAddOrEdit = "add";
            }
            else
            {
                CurrentProduct = _product;
                FlagAddOrEdit = "edit";
            }

            DataContext = CurrentProduct;

            Init();
        }

        public void Init()
        {
            try
            {
                CategoryComboBox.ItemsSource = Data.Trade2Entities.GetContext().Category.ToList();

                if (FlagAddOrEdit == "add")
                {
                    IdTextBox.Visibility = Visibility.Hidden;
                    IdLabel.Visibility = Visibility.Hidden;
                    NameTextBox.Text = string.Empty;
                    CategoryComboBox.SelectedItem = null;
                    //ProductImage
                    UnitTextBox.Text = string.Empty;
                    SupplierTextBox.Text = string.Empty;
                    CostTextBox.Text = string.Empty;
                    QuantityTextBox.Text = string.Empty;
                    DescriptionTextBox.Text = string.Empty;
                }
                else if(FlagAddOrEdit == "edit")
                {
                    IdTextBox.Visibility = Visibility.Visible;
                    IdLabel.Visibility = Visibility.Visible;

                    IdTextBox.Text = CurrentProduct.Id.ToString();
                    NameTextBox.Text = CurrentProduct.ProductName.Name;
                    //CategoryComboBox.SelectedItem = Data.Trade2Entities.GetContext().Category.Where(d => d.Id = CurrentProduct.IdProductCategory).FirstOrDefault();
                    //ProductImage
                    //UnitTextBox.Text = CurrentProduct.ProductUnit.Name;
                    SupplierTextBox.Text = CurrentProduct.Supplier.Name;
                    CostTextBox.Text = CurrentProduct.ProductCost.ToString();
                    QuantityTextBox.Text = CurrentProduct.ProductQuantityInStock.ToString();
                    DescriptionTextBox.Text = CurrentProduct.ProductDescription;
                }

            }
            catch(Exception ex)
            {

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Manager.MainFrame.CanGoBack)
            {
                Classes.Manager.MainFrame.GoBack();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
