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
        public Board board = new();

        Point a = new Point(1, 1);
        
        public MainWindow()
        {
            InitializeComponent();
            setStartingPosition();
        }

        private void setStartingPosition()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button();
                    button.Click += Button_Click;
                    
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    button.Name = "button" + i + j;
                    chessBoard.Children.Add(button);
                    Grid tempGrid = new();
                }
            }
            displayPieces();
        }

        private void displayPieces()
        {
            new List<Board>().GetEnumerator();
            foreach (ChessPiece piece in board)
            {
                Button button = (Button)chessBoard.Children[piece.CurrentPosition.x * 8 + piece.CurrentPosition.y];

                //Image image = new Image();
                //image.Source = new ImageSourceConverter().ConvertFromString(piece.imageSource) as ImageSource;
                //button.Content = image;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Grid.GetRow((Button)sender), Grid.GetColumn((Button)sender));
            
            return;
        }

        private UIElement GetPieceByGridIdx(int row, int column)
        {
            return chessBoard.Children
            .Cast<UIElement>()
            .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
        }
    }
    
}
