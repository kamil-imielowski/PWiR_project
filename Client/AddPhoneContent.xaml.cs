using SharedLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
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

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy AddPhoneContent.xaml
    /// </summary>
    public partial class AddPhoneContent : UserControl
    {
        public AddPhoneContent()
        {
            InitializeComponent();

            ComboBox cb = (ComboBox)FindName("Brand");
            ComboBoxItem cbi;

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
            Label lb = (Label)FindName("alert");
            lb.Foreground = Brushes.Red;
            //select brand
            ComboBox cb = (ComboBox)FindName("Brand");
            if (cb.SelectedItem == null)
            {
                lb.Content = "Wybierz markę";
                return;
            }
            ComboBoxItem cbi = (ComboBoxItem)cb.SelectedItem;
            Brand brand = (Brand)cbi.Tag;
            Phone phone = new Phone();
            phone.Brand = brand;

            TextBox modelTB, processorTB, ramTB, memoryTB, graphicTB, descriptionTB, priceTB;
            //string model, processor, ram, memory, graphic, description;
            double price;

            modelTB = (TextBox)FindName("model");
            if (String.IsNullOrEmpty(modelTB.Text) || modelTB.Text.Equals("Podaj model"))
            {
                lb.Content = "Podaj nazwę modelu";
                return;
            }
            phone.Model = modelTB.Text;

            processorTB = (TextBox)FindName("processor");
            if (String.IsNullOrEmpty(processorTB.Text) || processorTB.Text.Equals("Podaj procesor"))
            {
                lb.Content = "Podaj dane procesora";
                return;
            }
            phone.Processor = processorTB.Text;

            ramTB = (TextBox)FindName("ram");
            if (String.IsNullOrEmpty(ramTB.Text) || ramTB.Text.Equals("Podaj ram"))
            {
                lb.Content = "Podaj dane pamięci ram";
                return;
            }
            phone.Ram = ramTB.Text;

            memoryTB = (TextBox)FindName("memory");
            if (String.IsNullOrEmpty(memoryTB.Text) || memoryTB.Text.Equals("Podaj pamięć wbudowaną"))
            {
                lb.Content = "Podaj ilość pamięci wbudowanej";
                return;
            }
            phone.Memory = memoryTB.Text;

            graphicTB = (TextBox)FindName("graphic");
            if (graphicTB.Text.Equals("Podaj karte graficzną"))
                graphicTB.Text = "";
            phone.Graphic = graphicTB.Text;

            descriptionTB = (TextBox)FindName("description");
            if (String.IsNullOrEmpty(descriptionTB.Text) || descriptionTB.Text.Equals("Podaj opis"))
            {
                lb.Content = "Podaj opis urządzenia";
                return;
            }
            phone.Description = descriptionTB.Text;

            priceTB = (TextBox)FindName("price");
            if (Double.TryParse(priceTB.Text, NumberStyles.Currency, CultureInfo.CreateSpecificCulture("pl-PL"), out price))
            {
                phone.Price = price;
                lb.Content = phone.Price;
            }
            else
            {
                lb.Content = "Podaj cenę w formacie zł,gr";
                return;
            }

            DatePicker premiereDP = (DatePicker)FindName("premiere");
            if (string.IsNullOrEmpty(premiereDP.Text) || premiereDP.SelectedDate.Value > DateTime.Now)
            {
                lb.Content = "Podaj prawidłową datę premiery";
                premiereDP.Focus();
                return;
            }
            phone.Premiere = premiereDP.SelectedDate.Value;

            Uri adres = new Uri("http://localhost:2222/Test");
            using (var c = new ChannelFactory<IContract>(new BasicHttpBinding(), new EndpointAddress(adres)))
            {

                var s = c.CreateChannel();
                var results = s.AddPhone(phone);
                string result;
                if (results)
                {
                    lb.Foreground = Brushes.Green;
                    result = "Zapisano telfon!";
                }
                else
                    result = "Coś poszło nie tak!";
                lb.Content = result;

            }
        }

        private void price_LostFocus(object sender, RoutedEventArgs e)
        {
            //walidacja ceny
            Double value;
            if (Double.TryParse(price.Text, out value))
                price.Text = String.Format(CultureInfo.CreateSpecificCulture("pl-PL"), "{0:C2}", value);
                //price.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", value);
            else
                price.Text = String.Empty;
        }
    }
}
