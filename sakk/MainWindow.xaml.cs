using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
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
        int?[] timeOptions = { 1, 5, 10, 30, null };
        public bool gameInProgress = false;
        List<ChessPiece> pieces = new();
        Border startButtonBorder = new Border();
        public static Board board = new();
        Border timeBorder = new Border();
        public bool IsWhiteTurn = true;
        bool isPositionSetup = false;
        public static MainWindow _instance;
        public bool TimeExpanded = false;
        int? selectedTime = 10;
        DispatcherTimer Timer = new DispatcherTimer();
        Image dragDropImage;
        bool create = true;


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
            Button timer = (Button)(IsWhiteTurn ? lbWhiteTimer.Child : lbBlackTimer.Child);
            string[] time = timer.Content.ToString().Split(":");
            int minutes = int.Parse(time[0]);
            int seconds = int.Parse(time[1]);
            
            if (seconds == 0)
            {
                if (minutes == 0)
                {
                    if(isInsufficientMaterial(IsWhiteTurn))
                    {
                        handleWin(null);
                        return;
                    }
                    handleWin(!IsWhiteTurn);
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

        private bool isInsufficientMaterial(bool white)
        {
            bool isInsufficient = board.Pieces.Find(
                        x => x.IsWhite == white &&
                        (x.GetType() == typeof(Rook)
                        || x.GetType() == typeof(Pawn)
                        || x.GetType() == typeof(Queen)
                        )) == null;
            if (!isInsufficient)
            {
                return false;
            }
            return board.Pieces.FindAll(x => x.IsWhite == white &&(x.GetType() == typeof(Knight)|| x.GetType() == typeof(Bishop))).Count <2;
            
        }

        
        private void handleWin(bool? isWhiteWon)
        {

            //if (isWhiteWon)
            
            if (isWhiteWon is true)
            {
                MessageBox.Show("White won!");
            }
            else if (isWhiteWon is false)
            {
                MessageBox.Show("Black won!");
            }
            else
            {
                MessageBox.Show("Draw!");
            }
            
            Timer.Stop();
            gameInProgress = false;

        }

        private void createMenu()
        {
            createPassAndPlayButton();
            createCustomPositionButton();
        }

        private void createTimeButton()
        {
            timeBorder.Name = "timeBorder";
            timeBorder.Margin = new Thickness(20, 100, 20, 20);
            timeBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
            timeBorder.BorderThickness = new Thickness(10);
            timeBorder.CornerRadius = new CornerRadius(8);
            timeBorder.SetValue(Grid.RowProperty, 0);

            Button timeButton = new Button();
            timeButton.Name = "timeButton";
            timeButton.Style = (Style)FindResource("NoHoverButton");
            timeButton.Content = "10 perc 🠻";
            timeButton.Background = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
            timeButton.Foreground = new SolidColorBrush(Color.FromRgb(0xc3, 0xc3, 0xc3));
            timeButton.FontSize = 30;
            timeButton.BorderBrush = null;
            timeButton.Click += displayTimeOptions;

            timeBorder.Child = timeButton;
            menuGrid.Children.Add(timeBorder);
        }

        private void createPassAndPlayButton()
        {
            Border border = new Border();
            border.Name = "PassAndPlayBorder";
            border.Margin = new Thickness(20, 100, 20, 20);

            border.SetValue(Grid.RowProperty, 0);

            Button button = new Button();
            button.Name = "PassAndPlayButton";
            button.Content = "Pass and Play";
            button.Click += createPassAndPlayMenu;
            addBlackButtonStyles(button, border);
            menuGrid.Children.Add(border);
        }
        private void createPassAndPlayButton(string content, int rowProperty)
        {
            Border border = new Border();
            border.Name = "PassAndPlayBorder";
            border.Margin = new Thickness(20, 100, 20, 20);

            border.SetValue(Grid.RowProperty, rowProperty);

            Button button = new Button();
            button.Name = "PassAndPlayButton";
            button.Content = content;
            button.Click += conditionalPassAndPlayMenu;
            addBlackButtonStyles(button, border);
            menuGrid.Children.Add(border);
        }

        private void conditionalPassAndPlayMenu(object sender, RoutedEventArgs e)
        {
            if (board.isLegalPosition(IsWhiteTurn))
            {
                createPassAndPlayMenu(sender, e);

            }
            else {
                MessageBox.Show("Illegal position!");
            }


            
        }

        private void createCustomPositionButton()
        {
            Border border = new Border();
            border.Name = "CustomPositionBorder";
            border.Margin = new Thickness(20, 20, 20, 100);
            border.SetValue(Grid.RowProperty, 1);

            Button button = new Button();
            button.Name = "CustomPositionButton";
            button.Content = "Custom Position";
            button.Click += createCustomPositionMenu;
            addBlackButtonStyles(button, border);
            menuGrid.Children.Add(border);
        }

        private void createCustomPositionMenu(object sender, RoutedEventArgs e)
        {
            isPositionSetup = true;
            menuGrid.Children.Clear();
            addPieceSelection();
            createControlsPanel();
            createPassAndPlayButton("Start Game", 3);
            
        }

        private void createControlsPanel()
        {
            // Create the grid
            Grid grid = new Grid();
            grid.Name = "CastlingControlsGrid";
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            Grid.SetRow(grid, 2);

            // Create the radio buttons
            RadioButton blackRadioButton = new RadioButton();
            blackRadioButton.Margin = new Thickness(10, 0, 0, 0);
            blackRadioButton.FontSize = 15;
            blackRadioButton.Content = "Black to play";
            blackRadioButton.Foreground = Brushes.White;
            blackRadioButton.Height = 40;
            blackRadioButton.Width = 130;
            blackRadioButton.GroupName = "CurrentPlayer";
            Grid.SetRow(blackRadioButton, 0);
            blackRadioButton.Click += changeCurrentPlayer;

            RadioButton whiteRadioButton = new RadioButton();
            whiteRadioButton.FontSize = 15;
            whiteRadioButton.Content = "White to play";
            whiteRadioButton.Foreground = Brushes.White;
            whiteRadioButton.Height = 40;
            whiteRadioButton.Width = 140;
            whiteRadioButton.GroupName = "CurrentPlayer";
            whiteRadioButton.IsChecked = true;
            Grid.SetRow(whiteRadioButton, 0);
            Grid.SetColumn(whiteRadioButton, 1);
            whiteRadioButton.Click += changeCurrentPlayer;

            // Create the checkbox for White
            Label whiteLabel = new Label();
            whiteLabel.FontSize = 15;
            whiteLabel.Margin = new Thickness(10, 0, 0, 0);
            whiteLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            whiteLabel.Content = "White:";
            whiteLabel.Foreground = Brushes.White;
            Grid.SetRow(whiteLabel, 1);

            CheckBox whiteCheckBox1 = new CheckBox();
            whiteCheckBox1.FontSize = 15;
            whiteCheckBox1.Margin = new Thickness(50, 0, 50, 0);
            whiteCheckBox1.Content = "0-0";
            whiteCheckBox1.Tag = "white";
            whiteCheckBox1.Name = "whiteCheckBox1";
            whiteCheckBox1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            whiteCheckBox1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            whiteCheckBox1.Foreground = Brushes.White;
            Grid.SetRow(whiteCheckBox1, 1);
            Grid.SetColumn(whiteCheckBox1, 1);
            whiteCheckBox1.Click += changeCastlingRights;
            whiteCheckBox1.IsChecked = true;

            CheckBox whiteCheckBox2 = new CheckBox();
            whiteCheckBox2.FontSize = 15;
            whiteCheckBox2.Content = "0-0-0";
            whiteCheckBox2.Tag = "white";
            whiteCheckBox2.Name = "whiteCheckBox2";
            whiteCheckBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            whiteCheckBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            whiteCheckBox2.Foreground = Brushes.White;
            Grid.SetRow(whiteCheckBox2, 1);
            Grid.SetColumn(whiteCheckBox2, 2);
            whiteCheckBox2.Click += changeCastlingRights;
            whiteCheckBox2.IsChecked = true;
            

            // Create the checkbox for Black
            Label blackLabel = new Label();
            blackLabel.FontSize = 15;
            blackLabel.Margin = new Thickness(10, 0, 0, 0);
            blackLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            blackLabel.Content = "Black:";
            blackLabel.Foreground = Brushes.White;
            Grid.SetRow(blackLabel, 2);

            CheckBox blackCheckBox1 = new CheckBox();
            blackCheckBox1.FontSize = 15;
            blackCheckBox1.Margin = new Thickness(50, 0, 50, 0);
            blackCheckBox1.Content = "0-0";
            blackCheckBox1.Tag = "black";
            blackCheckBox1.Name = "blackCheckBox1";
            blackCheckBox1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            blackCheckBox1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            blackCheckBox1.Foreground = Brushes.White;
            Grid.SetRow(blackCheckBox1, 2);
            Grid.SetColumn(blackCheckBox1, 1);
            blackCheckBox1.Click += changeCastlingRights;
            blackCheckBox1.IsChecked = true;

            CheckBox blackCheckBox2 = new CheckBox();
            blackCheckBox2.FontSize = 15;
            blackCheckBox2.Content = "0-0-0";
                blackCheckBox2.Tag = "black";
            blackCheckBox2.Name = "blackCheckBox2";
            blackCheckBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            blackCheckBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            blackCheckBox2.Foreground = Brushes.White;
            Grid.SetRow(blackCheckBox2, 2);
            Grid.SetColumn(blackCheckBox2, 2);
            blackCheckBox2.Click += changeCastlingRights;
            blackCheckBox2.IsChecked = true;
            

            //adding each of this to separate stack panels
            StackPanel stackPanel1 = new StackPanel();
            stackPanel1.Orientation = Orientation.Horizontal;
            Grid.SetRow(stackPanel1, 0);
            stackPanel1.Children.Add(blackRadioButton);
            stackPanel1.Children.Add(whiteRadioButton);


            StackPanel stackPanel2 = new StackPanel();
            stackPanel2.Orientation = Orientation.Horizontal;
            Grid.SetRow(stackPanel2, 1);
            stackPanel2.Children.Add(whiteLabel);
            stackPanel2.Children.Add(whiteCheckBox1);
            stackPanel2.Children.Add(whiteCheckBox2);

            StackPanel stackPanel3 = new StackPanel();
            stackPanel3.Orientation = Orientation.Horizontal;
            Grid.SetRow(stackPanel3, 2);
            stackPanel3.Children.Add(blackLabel);
            stackPanel3.Children.Add(blackCheckBox1);
                stackPanel3.Children.Add(blackCheckBox2);

            
            grid.Children.Add(stackPanel1);
            grid.Children.Add(stackPanel2);
            grid.Children.Add(stackPanel3);




            menuGrid.Children.Add(grid);
        }

        private void changeCastlingRights(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox checkBox = sender as CheckBox;
                int row = checkBox.Tag.ToString() == "white" ? 7 : 0;
                int col = checkBox.Content.ToString() == "0-0" ? 7 : 0;
                ChessPiece piece = board[col,row];
                if (piece == null)
                {
                    checkBox.IsChecked = false;
                    return;
                }
                if (piece.GetType() == typeof(Rook))
                {
                    Rook rook = (Rook)piece;
                    rook.IsFirstMove = checkBox.IsChecked == true;
                }
                
            }
        }

        private void changeCurrentPlayer(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton radioButton = sender as RadioButton;
                IsWhiteTurn = radioButton.Content.ToString() != "Black to play";
                
            }
        }

        private void addPieceSelection()
        {
            WrapPanel wrapPanelWhite = new WrapPanel();
            wrapPanelWhite.Name = "WhitepieceSelection";
            Grid.SetRow(wrapPanelWhite, 0);
            WrapPanel wrapPanelBlack = new WrapPanel();
            wrapPanelBlack.Name = "BlackpieceSelection";
            Grid.SetRow(wrapPanelBlack, 1);
            wrapPanelWhite.Margin = new Thickness(20, 20, 20, 0);
            wrapPanelBlack.Margin = new Thickness(20, 20, 20, 0);
            
            foreach (BitmapImage pieceImg in ChessPiece.bitmapImages)
            {

                Image img = new Image();
                img.MouseDown += drag_drop;
                img.Width = 70;
                img.Source = pieceImg;
                //MessageBox.Show($"{pieceImg.UriSource.ToString()[pieceImg.UriSource.ToString().Length-7]}");
                
                img.Tag = "create";
                if (pieceImg.UriSource.ToString()[pieceImg.UriSource.ToString().Length - 6] == 'w')
                {
                    wrapPanelWhite.Children.Add(img);
                }
                else
                {
                    wrapPanelBlack.Children.Add(img);
                }
                
            }
            menuGrid.Children.Add(wrapPanelWhite);
            menuGrid.Children.Add(wrapPanelBlack);
        }

        private void createPassAndPlayMenu(object sender, RoutedEventArgs e)
        {
            
            menuGrid.Children.Clear();
            createStartButton();
            createTimeButton();
        }

        private void addBlackButtonStyles(Button button, Border border) 
            {
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
            border.BorderThickness = new Thickness(10);
            border.CornerRadius = new CornerRadius(8);
            button.Style = (Style)FindResource("NoHoverButton");
            button.Background = new SolidColorBrush(Color.FromRgb(0x17, 0x17, 0x17));
            button.Foreground = new SolidColorBrush(Color.FromRgb(0xc3, 0xc3, 0xc3));
            button.FontSize = 30;
            button.BorderBrush = null;
            border.Child = button;
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
                    button.MouseRightButtonDown += Button_MouseRightButtonDown;
                    button.BorderThickness = new Thickness(0);
                    button.Style =  (Style)FindResource("NoHoverButton");
                    button.AllowDrop = true;
                    button.Drop += button_drop;
                    

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

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (isPositionSetup)
            {
                Point p = new Point(Grid.GetColumn((Button)sender), Grid.GetRow((Button)sender));
                ChessPiece piece = board[p];
                if (piece == null)
                {
                    return;
                }
                updateCheckboxes(piece);
                board.Remove(p);
                displayPieces();
            }
            
        }

        private void updateCheckboxes(ChessPiece piece)
        {
            if ( piece == null || !(piece.GetType() == typeof(Rook) && ((Rook)piece).IsFirstMove == true))
            {
                return;
            }
            
                Rook rook = (Rook)piece;
                Grid cbGrid = (menuGrid.Children[2] as Grid);
                StackPanel wp = cbGrid.Children[rook.IsWhite ? 1 : 2] as StackPanel;
                CheckBox cb = wp.Children[rook.CurrentPosition.x == 0 ? 1 : 2] as CheckBox;
                cb.IsChecked = false;

        }

        private void button_drop(object sender, DragEventArgs e)
        {
            Grid btngrid = ((Button)sender).Content as Grid;
            Point p = new Point(Grid.GetColumn((Button)sender), Grid.GetRow((Button)sender));

            if (board.LegalMoves.Exists(x => x == p))
            {

                MovePiece(board.selectedPiece.CurrentPosition!, p);
                removeRemovables();
            }
            else if (isPositionSetup) {
                if (create)
                {
                    createPiece(p);
                }
                else
                {
                    relocatePiece(board.selectedPiece.CurrentPosition!, p);
                }
               
            
            }
            


        }

        private void createPiece(Point to)
        {
            if(board.selectedPiece == null)
            {
                return;
            }
            Type type = board.selectedPiece.GetType();
            ChessPiece piece = (ChessPiece)Activator.CreateInstance(type, to, board.selectedPiece.IsWhite);
            
            board[to] = piece;
            if (piece.GetType() == typeof(Pawn))
            {
                if (piece.CurrentPosition.y != (piece.IsWhite ? 6 : 1))
                {
                    (piece as Pawn).IsFirstMove = false;
                    if (piece.CurrentPosition.y == (piece.IsWhite ? 4 : 3))
                    {
                        (piece as Pawn).mayBePassanted = true;
                    }
                }
            }
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
            foreach (UIElement b in chessBoard.Children)
            {
                if (b is Button)
                {
                    Grid buttonGrid = ((Button)b).Content as Grid;
                    buttonGrid.Children.Clear();
                }
                
            }

            foreach (ChessPiece piece in board)
            {
                Button button = (Button)chessBoard.Children[piece.CurrentPosition.y * 8 + piece.CurrentPosition.x];
                Grid buttonGrid = button.Content as Grid;
                
                Image img = new Image();
                img.MouseDown += drag_drop;
                img.Source = piece.ImageByIdx;
                buttonGrid.Children.Add(img);
                string source = (img.Source as BitmapImage).UriSource.ToString();
                
                img.Tag = "move";

            }
        }

        private void MovePiece(Point from, Point to) {
            board[to] = board[from];

            board.LegalMoves.Clear();
            Button button = (Button)chessBoard.Children[from.y * 8 + from.x];
            Grid buttonGrid = button.Content as Grid;
            buttonGrid.Children.Clear();
            button = (Button)chessBoard.Children[to.y * 8 + to.x];
            buttonGrid = button.Content as Grid;
            buttonGrid.Children.Clear();
            Image img = new Image();

            img.Source = board[to].ImageByIdx;

            img.Tag = "move";
            board.selectedPiece = null;
            buttonGrid.Children.Add(img);
            img.MouseDown += drag_drop;

            IsWhiteTurn = !IsWhiteTurn;



            if (board.Pieces.FindAll(x => x is King).Count < 2)
            {
                if (board.Pieces.FindAll(x => x is King).Count == 0)
                {
                    handleWin(null);
                }
                else
                {
                    handleWin(board.Pieces.Find(x => x is King).IsWhite);
                }
                return;
            }
            if (isInsufficientMaterial(true) && isInsufficientMaterial(false))
            {
                handleWin(null);
                return;
            }

            



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
                    IsWhiteTurn = !IsWhiteTurn;
                    gameInProgress = false;
                }
            }
            else if (board[to] is King) 
            {
                var tempPiece = (King)board[to];
                if (Math.Abs(from.x - to.x) > 1)
                {
                    int row = tempPiece.IsWhite ? 7 : 0;
                    if (to.x > 4)
                    {
                        MovePiece(new Point(7, row), new Point(5, row));
                    }
                    else
                    {
                        MovePiece(new Point(0, row), new Point(3, row));
                    }
                    IsWhiteTurn ^= true;
                }
                tempPiece.IsFirstMove = false;
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
            checkIfWin();

            
           
            

            resetBoardColor();

        }

        private void checkIfWin()
        {
            if (!board.GetAvailablePieces(IsWhiteTurn).Any())
            {
                if (board[board.FindKingPoint(IsWhiteTurn)].HasAttacker(board))
                {
                    handleWin(!IsWhiteTurn);
                }
                else
                {
                    handleWin(null);
                }
            };
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
            IsWhiteTurn = !IsWhiteTurn;
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
            img.MouseDown += drag_drop;
            
            img.Tag = "move";
            btnGrid.Children.Add(img);
            gameInProgress = true;

            checkIfWin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (gameInProgress)
            {
                removeRemovables();
                Point p = new Point(Grid.GetColumn((Button)sender), Grid.GetRow((Button)sender));
                if (board[p] != null && board[p]!.IsWhite == IsWhiteTurn)
                {
                    board.LegalMoves.Clear();
                    board.selectedPiece = board[p]!;

                    resetBoardColor();
                    displayMoveOptions();
                    setupDragAndDrop(sender);

                }
                else if (board.LegalMoves.Exists(x => x == p))
                {

                    MovePiece(board.selectedPiece.CurrentPosition!, p);
                }
            }
            
            else if (isPositionSetup) {
                Point p = new Point(Grid.GetColumn((Button)sender), Grid.GetRow((Button)sender));
                if (create)
                {
                    
                    createPiece(p);
                }
                else
                {
                    
                        relocatePiece(board.selectedPiece.CurrentPosition, p);
                    
                    
                    
                }

            }
            



        }

        private void relocatePiece(Point from, Point to)
        {

            if (board[from] == null)
            {
                return;
            }
            updateCheckboxes(board[to]);
            updateCheckboxes(board[from]);


            board[to] = board[from];
            board.selectedPiece.CurrentPosition = to;
            Button button = (Button)chessBoard.Children[from.y * 8 + from.x];
            Grid buttonGrid = button.Content as Grid;
            buttonGrid.Children.Clear();
            button = (Button)chessBoard.Children[to.y * 8 + to.x];
            buttonGrid = button.Content as Grid;
            buttonGrid.Children.Clear();
            Image img = new Image();

            img.Source = board[to].ImageByIdx;

            img.Tag = "move";
            buttonGrid.Children.Add(img);
            img.MouseDown += drag_drop;
            if (board.selectedPiece.GetType() == typeof(Pawn) )
            {
                if (board.selectedPiece.CurrentPosition.y != (board.selectedPiece.IsWhite ? 1 : 6))
                {
                    (board.selectedPiece as Pawn).IsFirstMove = false;
                    if (board.selectedPiece.CurrentPosition.y == (board.selectedPiece.IsWhite ? 3 : 4))
                    {
                        (board.selectedPiece as Pawn).mayBePassanted = true;
                    }
                }
            }



            resetBoardColor();
        }

        private void setupDragAndDrop(object sender)
        {
            dragDropImage = (Image)((Grid)((ContentControl)sender).Content).Children[0];
            
             
        }

        private void drag_drop(object sender, MouseEventArgs e)
        {
            if (gameInProgress)
            {
                Image img = (Image)sender;
                if (img.Parent == null)
                {
                    return;
                }
                Button clickedButton = ((Grid)img.Parent).Parent as Button;
                Point p = new Point(Grid.GetColumn(clickedButton), Grid.GetRow(clickedButton));
                
                if (IsWhiteTurn != board[p].IsWhite)
                {
                    return;
                }

                board.LegalMoves.Clear();
                board.selectedPiece = board[p]!;
                displayMoveOptions();
                DragDrop.DoDragDrop(img, img, DragDropEffects.Move);
            }
            else if (isPositionSetup) {
                Image img = (Image)sender;
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    return;
                }

                create = img.Tag.ToString() == "create";
                Point p = new Point(-1, -1) ;
                if (!create)
                {
                    Button clickedButton = ((Grid)((Image)sender).Parent).Parent as Button;
                    p = new Point(Grid.GetColumn(clickedButton), Grid.GetRow(clickedButton));
                }
                
                string source = (img.Source as BitmapImage).UriSource.ToString();
                bool color = source[source.Length-6] == 'w';
                char type = source[source.Length - 5];
                

                switch (type)
                {
                    case 'b':
                        board.selectedPiece = new Bishop(p, color);
                        break;
                    case 'n':
                        board.selectedPiece = new Knight(p, color);
                        break;
                    case 'k':
                        board.selectedPiece = new King(p, color);
                        break;
                    case 'q':
                        board.selectedPiece = new Queen(p, color);
                        break;
                    case 'p':
                        board.selectedPiece = new Pawn(p, color);
                        break;
                    case 'r':
                        board.selectedPiece = new Rook(p, color, false);
                        break;
                    default:
                        board.selectedPiece = new Bishop(p, color);
                        break;

                }
                DragDrop.DoDragDrop(img, img, DragDropEffects.Move);

            }
            
            

                


            



        }
        

        private void displayMoveOptions()
        {
            removeRemovables();
            foreach (Point pa in board.selectedPiece.GetMovesFinal(board))
            {
                board.LegalMoves.Add(pa);
                Grid buttonGrid = ((Button)chessBoard.Children[pa.y * 8 + pa.x]).Content as Grid;
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

        public UIElement GetPieceByGridIdx(int row, int column)
        {
            return chessBoard.Children
            .Cast<UIElement>()
            .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
        }

        

       

        private void StartGame(object sender, RoutedEventArgs e)
        {
            gameInProgress = true;
            isPositionSetup = false;
            create = false;
            menuGrid.Children.Clear();
            createBackToMainMenuButton();
            Timer.Start();
        }

        private void createBackToMainMenuButton()
        {
            Border border = new Border();
            border.Name = "BacktoMainMenuBorder";
            border.Margin = new Thickness(150, 10, 10, 150);

            border.SetValue(Grid.RowProperty, 0);

            
            
            
            menuGrid.Children.Add(border);

            Button button = new Button();
            button.Style = (Style)FindResource("NoHoverButton");
            button.Content = "↩";
            
            button.Click += backToMainMenu;
            addBlackButtonStyles(button, border);
            button.FontSize = 30;

        }

        private void backToMainMenu(object sender, RoutedEventArgs e)
        {
            gameInProgress = false;
            isPositionSetup = false;
            IsWhiteTurn = true;
            create = false;
            menuGrid.Children.Clear();
            createMenu();
            resetBoardColor();
            board.Clear();
            board.SetDefaultChessPosition();
            removeRemovables();
            
            displayPieces();
            Timer.Stop();
            ((Button)lbWhiteTimer.Child).Content = "10:00";
            ((Button)lbBlackTimer.Child).Content = "10:00";

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
