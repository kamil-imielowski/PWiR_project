using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Tab_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(10 + (150 * index), 0, 0, 0);
            
            switch (index)
            {
                case 0:
                    GridMain.Background = Brushes.BurlyWood;
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new SearchContent());
                    break;

                case 1:
                    GridMain.Background = Brushes.Gray;
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new AddContent());
                    break;

                case 2:
                    GridMain.Background = Brushes.LightSeaGreen;
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new AddPhoneContent());
                    break;
            }
        }


    }
}