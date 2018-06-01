using SharedLib;
using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy AddContent.xaml
    /// </summary>
    public partial class AddContent : UserControl
    {
        public AddContent()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Label lb = (Label)FindName("alert");
            TextBox tbproc = (TextBox)FindName("name");
            if (String.IsNullOrEmpty(tbproc.Text) || tbproc.Text.Equals("Nazwa marki"))
            {
                lb.Foreground = Brushes.Red;
                lb.Content = "Podaj nazwę modelu";
                return;
            }
            string name = tbproc.Text;
            Uri adres = new Uri("http://localhost:2222/Test");
            using (var c = new ChannelFactory<IContract>(new BasicHttpBinding(), new EndpointAddress(adres)))
            {

                var s = c.CreateChannel();
                var results = s.AddBrand(name);
                string result = results ? "Zapisano!" : "Coś poszło nie tak!";
                lb.Content = result;

            }


        }
    }
}
