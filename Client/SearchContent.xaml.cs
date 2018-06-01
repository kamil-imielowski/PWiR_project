using System;
using System.Windows;
using System.ServiceModel;
using System.Windows.Controls;
using SharedLib;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy SearchContent.xaml
    /// </summary>
    public partial class SearchContent : UserControl
    {
        public SearchContent()
        {
            InitializeComponent();

            ComboBox cb = (ComboBox)FindName("Brand");
            ComboBoxItem cbi;

            cb.Items.Add("Wszystkie");
            cb.SelectedIndex = 0;

            Uri adres = new Uri("http://localhost:2222/Test");
            using (var c = new ChannelFactory<IContract>(new BasicHttpBinding(), new EndpointAddress(adres)))
            {
                var s = c.CreateChannel();
                var results = s.GetBrands();
                foreach (Brand n in results)
                {
                    cbi = new ComboBoxItem();
                    //cbi.Tag = n.ID;
                    cbi.Tag = n;
                    cbi.Content = n.Name;
                    cb.Items.Add(cbi);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Label lb = (Label)FindName("test");
            Grid container = (Grid)FindName("GridContainer");
            try
            {
                container.Children.RemoveAt(5);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            ComboBox cb = (ComboBox)FindName("Brand");
            bool brandFilter = false, price1Filter = false, price2Filter = false;
            Brand brand = new Brand();
            if (cb.SelectedIndex > 0)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb.SelectedItem;
                brand = (Brand)cbi.Tag;
                brandFilter = true;
            }

            TextBox price1TB = (TextBox)FindName("price1");
            TextBox price2TB = (TextBox)FindName("price2");
            double price1, price2;
            if (Double.TryParse(price1TB.Text, NumberStyles.Currency, CultureInfo.CreateSpecificCulture("pl-PL"), out price1))
                price1Filter = true;
            if (Double.TryParse(price2TB.Text, NumberStyles.Currency, CultureInfo.CreateSpecificCulture("pl-PL"), out price2))
                price2Filter = true;

            if (price1Filter && price2Filter && price1 > price2)
            {
                lb.Content = "Błędnie wypełniony filtr";
                return;
            }

            Uri adres = new Uri("http://localhost:2222/Test");
            using (var c = new ChannelFactory<IContract>(new BasicHttpBinding(), new EndpointAddress(adres)))
            {
                var s = c.CreateChannel();
                var results = s.GetPhones();
                List<DisplayDataGrid> data = new List<DisplayDataGrid>();
                foreach (Phone p in results)
                {
                    DisplayDataGrid d = new DisplayDataGrid()
                    {
                        Brand = p.Brand.Name,
                        Model = p.Model,
                        Processor = p.Processor,
                        Ram = p.Ram,
                        Memory = p.Memory,
                        Graphic = p.Graphic,
                        Description = p.Description,
                        Price = p.Price,
                        Premiere = p.Premiere.ToString("MM-dd-yyyy")
                    };
                    data.Add(d);
                }
                if (brandFilter)
                    data = data.Where(x => x.Brand == brand.Name).ToList();

                if (price1Filter)
                    data = data.Where(x => x.Price > price1).ToList();

                if (price2Filter)
                    data = data.Where(x => x.Price < price2).ToList();

                if (data.Count == 0)
                {
                    lb.Content = "brak telefonów dla podanych kryteriów";
                    return;
                }
                DataGrid dg = new DataGrid()
                {
                    Name = "dg",
                    Margin = new Thickness(0, 70, 0, 0),
                    CanUserAddRows = false,
                    CanUserResizeColumns = true,
                    CanUserSortColumns = true,
                    IsReadOnly = true,
                    ItemsSource = data
                };
                container.Children.Add(dg);
            }
        }

        private void price_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox price = (TextBox)sender;
            //walidacja ceny
            Double value;
            if (Double.TryParse(price.Text, out value))
                price.Text = String.Format(CultureInfo.CreateSpecificCulture("pl-PL"), "{0:C2}", value);
            //price.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", value);
            else
            {
                switch (price.Name)
                {
                    case "price1":
                        price.Text = "Cena od (format zł,gr)";
                        break;
                    case "price2":
                        price.Text = "Cena do (format zł,gr)";
                        break;
                }
            }
        }

        private void price_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox price = (TextBox)sender;
            price.Text = "";
        }
    }

    public class DisplayDataGrid
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Processor { get; set; }
        public string Ram { get; set; }
        public string Memory { get; set; }
        public string Graphic { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Premiere { get; set; }
    }
}
