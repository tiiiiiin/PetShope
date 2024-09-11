using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace PetShop321.Classes
{
    public static class Manager
    {
        public static Frame MainFrame { get; set; }


        public static void GetImageData()
        {
            try
            {
                var list = Data.Trade2Entities.GetContext().Product.ToList();
                foreach (var item in list)
                {
                    string path = Directory.GetCurrentDirectory() + @"\img\" + item.PhotoName;
                    if (File.Exists(path))
                    {
                        item.ProductPhoto = File.ReadAllBytes(path);
                    }
                }
                Data.Trade2Entities.GetContext().SaveChanges();
            }
            catch (Exception)
            {

            }
        }
    }
}
