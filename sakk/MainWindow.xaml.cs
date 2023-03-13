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
using System.Windows.Ink;
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
        List<ChessPiece> pieces = new();
        
        public static Board board = new();

        public bool IsWhiteTurn = true;

        public static MainWindow _instance;

        public MainWindow()
        {
            _instance = this;
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
                    button.BorderThickness = new Thickness(0);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    button.Name = "button" + i + j;
                    chessBoard.Children.Add(button);
                    Grid tempGrid = new();
                    button.Content = tempGrid;
                }
            }
            resetBoardColor();
            displayPieces();
        }

        private void resetBoardColor()
        {
            BrushConverter bc = new();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = (Button)chessBoard.Children[i * 8 + j];
                    if ((i + j) % 2 == 0)
                    {
                        button.Background = Brushes.White;
                    }
                    else
                    {
                        button.Background = (Brush)bc.ConvertFrom("#C19A6B");
                    }
                }
            }

        }

        private void displayPieces()
        {
            new List<Board>().GetEnumerator();
            foreach (ChessPiece piece in board)
            {
                Button button = (Button)chessBoard.Children[piece.CurrentPosition.y * 8 + piece.CurrentPosition.x];
                Grid buttonGrid = button.Content as Grid;
                
                Image img = new Image();
                img.Source = piece.ImageByIdx;
                buttonGrid.Children.Add(img);


            }
        }

        private void MovePiece(Point from, Point to) {
            board[to] = board[from];
            if (board[to] is Pawn) {
                var tempPiece = (Pawn)board[to];
                if (tempPiece.IsFirstMove)
                {
                    tempPiece.mayBePassanted = tempPiece.CurrentPosition.y == (tempPiece.IsWhite ? 4 : 3);
                    tempPiece.IsFirstMove = false;
                }
                var checkPassantablePiece = board[new Point(to.x, to.y + (tempPiece.IsWhite ? 1 : -1))];
                if (checkPassantablePiece is Pawn && ((Pawn)checkPassantablePiece).mayBePassanted)
                {
                    board.Remove(checkPassantablePiece);
                }
            }
            else if (board[to] is King) { 
                               ((King)board[to]).IsFirstMove = false;
                       }
            else if (board[to] is Rook) {
                               ((Rook)board[to]).IsFirstMove = false;
                       }
            foreach (ChessPiece piece in board)
            {
                if (piece.IsWhite != IsWhiteTurn && piece is Pawn)
                {
                    ((Pawn)piece).mayBePassanted = false;
                }
            }
            board.LegalMoves.Clear();
            Button button = (Button)chessBoard.Children[from.y * 8 + from.x];
            Grid buttonGrid = button.Content as Grid;
            buttonGrid.Children.Clear();
            button = (Button)chessBoard.Children[to.y * 8 + to.x];
            buttonGrid = button.Content as Grid;
            buttonGrid.Children.Clear();
            Image img = new Image();
            
            img.Source = board[to].ImageByIdx;
            board.selectedPiece = null;
            buttonGrid.Children.Add(img);


            IsWhiteTurn = !IsWhiteTurn;
            resetBoardColor();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Grid.GetColumn((Button)sender), Grid.GetRow((Button)sender) );
            if (board[p]!=null && board[p]!.IsWhite == IsWhiteTurn)
            {
                board.LegalMoves.Clear();
                board.selectedPiece = board[p]!;

                resetBoardColor();
                foreach (Point pa in board.selectedPiece.GetMovesFinal(board))
                {
                    board.LegalMoves.Add(pa);
                    Grid buttonGrid =((Button)chessBoard.Children[pa.y * 8 + pa.x]).Content as Grid;
                    Ellipse elipse = new();
                    elipse.Stretch = Stretch.UniformToFill;
                    elipse.Tag = "removable";
                    if (buttonGrid.Children.Count > 0)
                    {
                        elipse.Stroke = Brushes.Gray;
                        elipse.StrokeThickness = 7;
                        Panel.SetZIndex(elipse, -1);


                    }
                    else
                    {
                        elipse.Fill = Brushes.Gray;
                        
                    }
                    buttonGrid.Children.Add(elipse);
                }
            }
            else if(board.LegalMoves.Exists(x => x == p))
            {

                MovePiece(board.selectedPiece.CurrentPosition!, p);

            }
            return;
        }

        public UIElement GetPieceByGridIdx(int row, int column)
        {
            return chessBoard.Children
            .Cast<UIElement>()
            .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
        }
    }
    
}
