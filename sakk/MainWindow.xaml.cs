using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace sakk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point a = new Point(1, 1);
        public MainWindow()
        {
            
            
            InitializeComponent();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button();
                    button.Click += Button_Click;
                    if ((i + j) % 2 == 0)
                    {
                        button.Background = Brushes.Black;
                    }
                    else
                    {
                        button.Background = Brushes.White;
                    }
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    chessBoard.Children.Add(button);
                }
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Grid.GetRow((Button)sender), Grid.GetColumn((Button)sender));
            
            return;
        }
    }
    
}
