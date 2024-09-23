using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
using System.IO;

namespace PetShop321.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public string FlagAddOrEdit = "default";
        public bool FlafPhoto = false;
        public Data.Product CurrentProduct = new Data.Product();

        public AdminPage AdminPage { get; set; }
        public Pages.ManagerPage ManagerPage { get; set; }
        public ClientPage ClientPage { get; set; }

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
                FlafPhoto = true;
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
                    FlafPhoto = true;
                    NameTextBox.Text = CurrentProduct.ProductName.Name;
                    CategoryComboBox.SelectedItem = Data.Trade2Entities.GetContext().Category.Where(d => d.Id == CurrentProduct.IdProductCategory).FirstOrDefault();
                    UnitTextBox.Text = CurrentProduct.Unites.Name;
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
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    errors.AppendLine("Заполните наименование");
                }
                if (CategoryComboBox.SelectedItem == null)
                {
                    errors.AppendLine("Выберите категорию");
                }
                if (string.IsNullOrEmpty(QuantityTextBox.Text))
                {
                    errors.AppendLine("Заполните наименование");
                }
                else
                {
                    var tryQuantity = Int32.TryParse(QuantityTextBox.Text, out var resultQuantity);
                    if (!tryQuantity)
                    {
                        errors.AppendLine("Количество - целое число");
                    }
                }
                if (string.IsNullOrEmpty(UnitTextBox.Text))
                {
                    errors.AppendLine("Заполните ед.измерения");
                }
                if (string.IsNullOrEmpty(SupplierTextBox.Text))
                {
                    errors.AppendLine("Заполните поставщика");
                }
                if (string.IsNullOrEmpty(CostTextBox.Text))
                {
                    errors.AppendLine("Заполните наименование");
                }
                else
                {
                    var tryCost = Decimal.TryParse(CostTextBox.Text, out var resultCost);
                    if (!tryCost)
                    {
                        errors.AppendLine("Стоимость должна быть числом");
                    }
                    else
                    {
                        // Проверить количество знаков после запятой (максимум 2 знака)
                        int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(resultCost)[3])[2];
                        if (decimalPlaces > 2)
                        {
                            errors.AppendLine("Стоимость может содержать не более 2 знаков после запятой");
                        }
                    }
                    if (tryCost && resultCost < 0)
                    {
                        errors.AppendLine("Стоимость не может быть отрицательной");
                    }
                }

                if (string.IsNullOrEmpty(DescriptionTextBox.Text))
                {
                    errors.AppendLine("Заполните описание");
                }
                if (FlafPhoto == false) 
                {
                    errors.AppendLine("Выберите изображение");
                }
               

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedCategory = CategoryComboBox.SelectedItem as Data.Category;
                CurrentProduct.IdProductCategory = selectedCategory.Id;

                CurrentProduct.ProductCost = Convert.ToDecimal(CostTextBox.Text);
                CurrentProduct.ProductQuantityInStock = Convert.ToInt32(QuantityTextBox.Text);
                CurrentProduct.ProductDescription = DescriptionTextBox.Text;

                var searchName = (from obj in Data.Trade2Entities.GetContext().ProductName
                                  where obj.Name == NameTextBox.Text
                                  select obj).FirstOrDefault();
                if(searchName != null)
                {
                    CurrentProduct.IdProductName = searchName.Id;
                }
                else
                {
                    Data.ProductName productName = new Data.ProductName() 
                    { 
                        Name = NameTextBox.Text
                    };
                    Data.Trade2Entities.GetContext().ProductName.Add(productName);
                    Data.Trade2Entities.GetContext().SaveChanges();

                    CurrentProduct.IdProductName = productName.Id;

                }

                var searchUnit = (from obj in Data.Trade2Entities.GetContext().Unites
                                  where obj.Name == UnitTextBox.Text
                                  select obj).FirstOrDefault();
                if (searchUnit != null)
                {
                    CurrentProduct.ProductUnit = searchUnit.Id;
                }
                else
                {
                    Data.Unites unitName = new Data.Unites()
                    {
                        Name = UnitTextBox.Text
                    };
                    Data.Trade2Entities.GetContext().Unites.Add(unitName);
                    Data.Trade2Entities.GetContext().SaveChanges();

                    CurrentProduct.ProductUnit = unitName.Id;

                }


                var searchSuppler = (from obj in Data.Trade2Entities.GetContext().Supplier
                                  where obj.Name == SupplierTextBox.Text
                                  select obj).FirstOrDefault();
                if (searchSuppler != null)
                {
                    CurrentProduct.IdProductSupplier = searchSuppler.Id;
                }
                else
                {
                    Data.Supplier supplierName = new Data.Supplier()
                    {
                        Name = SupplierTextBox.Text
                    };
                    Data.Trade2Entities.GetContext().Supplier.Add(supplierName);
                    Data.Trade2Entities.GetContext().SaveChanges();

                    CurrentProduct.IdProductSupplier = supplierName.Id;

                }
                // в бд поставить значения нуль для столбцов мануфактурер и тд/ проверить арктикул

                if (FlagAddOrEdit == "edit")
                {
                    Data.Trade2Entities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно сохранено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (FlagAddOrEdit == "add")
                {
                    Data.Trade2Entities.GetContext().Product.Add(CurrentProduct);
                    Data.Trade2Entities.GetContext().SaveChanges();

                    MessageBox.Show("Успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (AdminPage != null)
                {
                    AdminPage.Update();
                    AdminPage.init();
                }

                if (ClientPage != null)
                {
                    ClientPage.Update();
                    ClientPage.init();
                }

                if (ManagerPage != null)
                {
                    ManagerPage.Update();
                    ManagerPage.init();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProductImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
                try
                {
                    Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                    if (openFileDialog.ShowDialog() == true)
                    {
                        var imageSource = new BitmapImage(new Uri(openFileDialog.FileName));

                        if (imageSource.PixelWidth <= 300 && imageSource.PixelHeight <= 200)
                        {
                            FlafPhoto = true;
                            ProductImage.Source = imageSource;

                            byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                            string imageName = System.IO.Path.GetFileName(openFileDialog.FileName);

                            CurrentProduct.PhotoName = imageName;
                            CurrentProduct.ProductPhoto = imageBytes;
                        }
                        else
                        {
                            MessageBox.Show("Выберите изображение с разрешением не более 300x200 пикселей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выборе изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            
        }
    }
}
