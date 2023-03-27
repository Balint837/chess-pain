using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;


namespace sakk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WrapPanel wrapPanel = new WrapPanel();
        int?[] timeOptions = { 1, 5, 10,30,null };
        public bool gameInProgress = false;
        List<ChessPiece> pieces = new();
        Border startButtonBorder = new Border();
        public static Board board = new();
        Border timeBorder = new Border();
        public bool IsWhiteTurn = true;

        public static MainWindow _instance;
        public bool TimeExpanded = false;
        int? selectedTime = 10;
        DispatcherTimer Timer = new DispatcherTimer();




        public MainWindow()
        {
            
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            
            _instance = this;
            InitializeComponent();

            setStartingPosition();

            createMenu();

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Button timer = (Button)(IsWhiteTurn? lbWhiteTimer.Child : lbBlackTimer.Child);
            string[] time = timer.Content.ToString().Split(":");
            int minutes = int.Parse(time[0]);
            int seconds = int.Parse(time[1]);
            if (seconds == 0)
            {
                if (minutes == 0)
                {

                    handleWin(!IsWhiteTurn);
                    Timer.Stop();
                    return;
                }
                else
                {
                    minutes--;
                    seconds = 59;
                }
            }
            else
            {
                seconds--;
            }

            timer.Content = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        private void handleWin(bool isWhiteWon)
        {
            
            if (isWhiteWon)
            {
                MessageBox.Show("White won!");
            }
            else
            {
                MessageBox.Show("Black won!");
            }


        }

        private void createMenu()
        {
            createStartButton();
            createTimeButton();
        }

        private void createTimeButton()
        {
            timeBorder.Name = "timeBorder";
            timeBorder.Margin = new Thickness(20, 100, 20, 20);
            timeBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
            timeBorder.BorderThickness = new Thickness(10);
            timeBorder.CornerRadius = new CornerRadius(8);
            timeBorder.SetValue(Grid.RowProperty, 0);

            Button timeButton_Copy = new Button();
            timeButton_Copy.Name = "timeButton_Copy";
            timeButton_Copy.Style = (Style)FindResource("NoHoverButton");
            timeButton_Copy.Content = "10 perc 🠻";
            timeButton_Copy.Background = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
            timeButton_Copy.Foreground = new SolidColorBrush(Color.FromRgb(0xc3, 0xc3, 0xc3));
            timeButton_Copy.FontSize = 30;
            timeButton_Copy.BorderBrush = null;
            timeButton_Copy.Click += displayTimeOptions;

            timeBorder.Child = timeButton_Copy;
            menuGrid.Children.Add(timeBorder);
        }

        private void createStartButton()
        {
            
            startButtonBorder.Margin = new Thickness(15, 40, 15, 40);
            startButtonBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x54, 0xB1, 0x4D));
            startButtonBorder.BorderThickness = new Thickness(10);
            startButtonBorder.CornerRadius = new CornerRadius(8);
            startButtonBorder.Name = "startButtonWrapper";
            Grid.SetRow(startButtonBorder, 1);

            Button startButton = new Button();
            startButton.Name = "startButton";
            startButton.Content = "StartGame";
            startButton.Style = (Style)Application.Current.Resources["NoHoverButton"];
            startButton.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x54, 0xB1, 0x4D));
            startButton.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xF1, 0xED, 0xED));
            startButton.FontSize = 24;
            startButton.FontFamily = new FontFamily("Arial");
            startButton.FontWeight = FontWeights.Bold;
            startButton.BorderBrush = null;
            startButton.BorderThickness = new Thickness(0);
            startButton.Click += StartGame;
            startButton.Style = (Style)FindResource("NoHoverButton");
            startButtonBorder.Child = startButton;

            menuGrid.Children.Add(startButtonBorder);
        }



        
        private void setStartingPosition()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button();
                    button.Padding = new Thickness(7);
                    button.Click += Button_Click;
                    button.BorderThickness = new Thickness(0);
                    button.Style =  (Style)FindResource("NoHoverButton");
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
        private void removeRemovables() {
            foreach (UIElement element in chessBoard.Children)
            {
                if (element is Button)
                {
                    Grid buttonGrid = ((Button)element).Content as Grid;
                    foreach (UIElement el in buttonGrid.Children)
                    {
                        if (el is Ellipse && (string)((Ellipse)el).Tag == "removable")
                        {
                            buttonGrid.Children.Remove(el);
                            break;
                        }
                    }
                }
            }


        }

        private void displayPieces()
        {
            
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
                else if (to.y== (tempPiece.IsWhite ? 0 : 7))
                {
                    pawnPromotion(to, tempPiece.IsWhite);
                    gameInProgress = false;
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

            board[board.FindKingPoint(IsWhiteTurn)].GetMovesFinal(board);

            if ((board.IsMated == null) ? (false) : ((bool)board.IsMated))
            {
                MessageBox.Show($"{((bool)board.IsMated ? "white" : "black")} sucks lol");
            }
            
           
            resetBoardColor();

        }

        private void pawnPromotion(Point to, bool IsWhite)
        {
            BrushConverter bc = new();
            Grid gr = new Grid();
            gr.Background = (Brush)bc.ConvertFrom("#fff");
            gr.RowDefinitions.Add(new RowDefinition());
            gr.RowDefinitions.Add(new RowDefinition());
            gr.RowDefinitions.Add(new RowDefinition());
            gr.RowDefinitions.Add(new RowDefinition());
            Image img = new Image();
            img.Margin = new Thickness(5,10,5,10);
            img.Source = ChessPiece.bitmapImages[3 + (IsWhite ? 6 : 0)];
            img.Stretch = Stretch.UniformToFill;
            
            img.Tag = "queen";
            img.MouseDown += promotionPiece_Click;
            gr.Children.Add(img);
            Grid.SetRow(img, 0);
            img = new Image();
            img.Margin = new Thickness(5);

            img.Source = ChessPiece.bitmapImages[0 + (IsWhite ? 6 : 0)];
            img.Stretch = Stretch.UniformToFill;
            img.Tag = "rook";
            img.MouseDown += promotionPiece_Click;
            gr.Children.Add(img);
            Grid.SetRow(img, 1);
            img = new Image();
            img.Margin = new Thickness(5);

            img.Source = ChessPiece.bitmapImages[2 + (IsWhite ? 6 : 0)];
            img.Stretch = Stretch.UniformToFill;
            img.Tag = "bishop";
            img.MouseDown += promotionPiece_Click;
            gr.Children.Add(img);
            Grid.SetRow(img, 2);
            img = new Image();
            img.Margin = new Thickness(5);

            img.Source = ChessPiece.bitmapImages[1 + (IsWhite ? 6 : 0)];
            img.Stretch = Stretch.UniformToFill;
            img.Tag = "knight";
            img.MouseDown += promotionPiece_Click;
            gr.Children.Add(img);
            Grid.SetRow(img, 3);
            Grid.SetColumn(gr, to.x);
            Grid.SetRow(gr, to.y - (IsWhite ? 0 : 3));
            Grid.SetRowSpan(gr, 4);
            chessBoard.Children.Add(gr);

            
             
            

        }

        private void promotionPiece_Click(object sender, MouseButtonEventArgs e)
        {
            Image senderPiece = (Image)sender;
            ChessPiece newPiece;
            Point to = new Point(Grid.GetColumn(senderPiece.Parent as Grid), Grid.GetRow(senderPiece.Parent as Grid) + (IsWhiteTurn ? 3 : 0));
            switch (senderPiece.Tag)
            {
                case "queen":
                    newPiece = new Queen(to, !IsWhiteTurn);
                    break;
                case "rook":
                    newPiece = new Rook(to, !IsWhiteTurn, false);
                    break;
                case "bishop":
                    newPiece = new Bishop(to, !IsWhiteTurn);
                    break;
                case "knight":
                    newPiece = new Knight(to, !IsWhiteTurn);
                    break;
                    default:
                    newPiece = new Queen(to, !IsWhiteTurn);
                    break;

            }
            board[to] = newPiece;
            chessBoard.Children.Remove(senderPiece.Parent as Grid);
            Button btn = (Button)chessBoard.Children[to.y * 8 + to.x];
            Grid btnGrid = btn.Content as Grid;
            btnGrid.Children.Clear();
            Image img = new Image();
            img.Source = newPiece.ImageByIdx;
            btnGrid.Children.Add(img);
            gameInProgress = true;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!gameInProgress)
            {
                return;
            }

            removeRemovables();
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
                    
                    elipse.Opacity = 0.7;
                    elipse.Tag = "removable";
                    if (buttonGrid.Children.Count > 0 || (board.selectedPiece is Pawn && board.selectedPiece.CurrentPosition.x != pa.x))
                    {
                        elipse.Stroke = Brushes.Gray;
                        elipse.StrokeThickness = 7;
                        Panel.SetZIndex(elipse, -1);


                    }
                    
                    else
                    {
                        elipse.Fill = Brushes.Gray;
                        elipse.Width = 33;
                        elipse.Height = 33;
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

        

       

        private void StartGame(object sender, RoutedEventArgs e)
        {
            gameInProgress = true;
            menuGrid.Children.Clear();
            Timer.Start();
        }

        private void displayTimeOptions(object sender, RoutedEventArgs e)
        {

              
            if (!TimeExpanded)
            {
                Grid.SetRow(startButtonBorder, 2);

                wrapPanel.SetValue(Grid.RowProperty, 1);
                wrapPanel.Orientation = Orientation.Horizontal;

                bool isSecond = false;
                foreach (int? time in timeOptions)
                {

                    Border border = new Border();

                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
                    border.BorderThickness = new Thickness(10);
                    border.CornerRadius = new CornerRadius(8);

                    border.Margin = new Thickness(9, 0, 0, 10);

                    border.Width = time == null ? 269 : 130;
                    border.Height = 66;

                    Button button = new Button();
                    button.Style = (Style)FindResource("NoHoverButton");
                    button.Content = time == null ? "∞" : $"{time}";
                    button.Background = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
                    button.Foreground = new SolidColorBrush(Color.FromRgb(0xc3, 0xc3, 0xc3));
                    button.FontSize = 30;
                    button.BorderBrush = null;
                    button.Tag = time;
                    button.Margin = new Thickness(0, 0, 0, 0);
                    button.Click += setTime;

                    border.Child = button;
                    wrapPanel.Children.Add(border);
                    isSecond = !isSecond;
                }
                menuGrid.Children.Add(wrapPanel);
            }
            else
            {
                Grid.SetRow(startButtonBorder, 1);
                menuGrid.Children.Remove(wrapPanel);
            }
            TimeExpanded = !TimeExpanded;
        }

        private void setTime(object sender, RoutedEventArgs e)
        {
            displayTimeOptions(sender, e);
            Button mainButton = timeBorder.Child as Button;
            Button timeButton = sender as Button;
            mainButton.Tag = timeButton.Tag;
            selectedTime = (int?)timeButton.Tag;
           
            if (mainButton.Tag == null) {
                mainButton.Content = timeButton.Content + "🠻";
                lbWhiteTimer.Opacity = 0;
                lbBlackTimer.Opacity = 0;
            }
            else
            {
                lbWhiteTimer.Opacity = 1;
                lbBlackTimer.Opacity = 1;
                ((Button)lbWhiteTimer.Child).Content = timeButton.Content+ ":00";
                ((Button)lbBlackTimer.Child).Content = timeButton.Content+ ":00";
                mainButton.Content = timeButton.Content + " perc 🠻";
            }
        }
    }
    
}
