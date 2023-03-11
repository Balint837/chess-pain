﻿using System;
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
        List<ChessPiece> pieces = new();
        
        public Board board = new();

        public bool IsWhiteTurn = true;

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
            IsWhiteTurn = !IsWhiteTurn;
            board[to] = board[from];
            if (board[to].GetType() == typeof(Pawn)) {
                ((Pawn)board[to]).IsFirstMove = false;
            }
            else if (board[to].GetType() == typeof(King)) { 
                               ((King)board[to]).IsFirstMove = false;
                       }
            else if (board[to].GetType() == typeof(Rook)) {
                               ((Rook)board[to]).IsFirstMove = false;
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

        
                resetBoardColor();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Grid.GetColumn((Button)sender), Grid.GetRow((Button)sender) );
            if (board[p]!=null && board[p].IsWhite == IsWhiteTurn)
            {
                board.LegalMoves.Clear();
                board.selectedPiece = board[p];

                resetBoardColor();
                foreach (Point pa in board[p].GetMovesFinal(board))
                {
                    board.LegalMoves.Add(pa);

                    Button button = (Button)chessBoard.Children[pa.y * 8 + pa.x];
                    button.Background = Brushes.Black;
                }
            }
            else if(board.LegalMoves.Exists(x => x == p))
            {

                MovePiece(board.selectedPiece.CurrentPosition, p);

            }
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
