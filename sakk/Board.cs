using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;

namespace sakk
{
    public class Board
    {
        public List<ChessPiece> Pieces = new List<ChessPiece>();
        public List<Point> LegalMoves = new List<Point>();
        public ChessPiece selectedPiece;
        public bool? IsMated = null;
        public bool IsMain = true;

        public Point FindKingPoint(bool isWhite)
        {
            return Pieces.Find(x => x is King && x.IsWhite == isWhite)!.CurrentPosition!;
        }

        public static List<Point> QueenEndpoints(Board board, Point p, bool inclusiveStart = false, bool ignoreColor = false)
        {
            List<Point> result = new();
            ChessPiece? currentPiece = board[p];
            for (int xp = -1; xp < 2; xp++)
            {
                for (int yp = -1; yp < 2; yp++)
                {
                    if (xp == 0 && yp == 0) continue;
                    int x = p.x+xp;
                    int y = p.y+yp;
                    while (board[x,y] == null && x > -1 && x < 8 && y > -1 && y < 8)
                    {
                        x += xp;
                        y += yp;
                    }
                    bool forceEnter = false;
                    if (board[x,y] == null)
                    {
                        forceEnter = true;
                        x -= xp;
                        y -= yp;
                    }
                    if (currentPiece == null || forceEnter)
                    {
                        if (!(p.x == x && p.y == y))
                        {
                            result.Add(new Point(x,y));
                        }
                    }
                    else
                    {
                        if (ignoreColor || (currentPiece.IsWhite != board[x, y]!.IsWhite))
                        {
                            result.Add(new Point(x, y));
                        }
                        else
                        {
                            x -= xp;
                            y -= yp;
                            if (!(p.x == x && p.y == y))
                            {
                                result.Add(new Point(x, y));
                            }
                        }
                    }
                }
            }
            if (inclusiveStart)
            {
                result.Add(new Point(p.x, p.y));
            }
            return Utils.FilterPoints(result);
        }
        public static List<Point> KnightMoves(Board board, Point p, bool? isWhite = null)
        {
            List<Point> result = new List<Point>
            {
                new Point(p.x + 1, p.y + 2),
                new Point(p.x + 1, p.y - 2),
                new Point(p.x - 1, p.y + 2),
                new Point(p.x - 1, p.y - 2),
                new Point(p.x + 2, p.y + 1),
                new Point(p.x + 2, p.y - 1),
                new Point(p.x - 2, p.y + 1),
                new Point(p.x - 2, p.y - 1)
            };
            if (isWhite != null)
            {
                result = result.Where(x => board[x] == null || (board[x]!.IsWhite != isWhite)).ToList();
            }
            else if (board[p] != null)
            {
                result = result.Where(x => board[x] == null || (board[x]!.IsWhite != board[p]!.IsWhite)).ToList();
            }
            return Utils.FilterPoints(result);
        }

        public static List<Point> DrawSection(Board board, Point p1, Point p2, bool forceInclusiveEnd = false, bool forceInclusiveStart = false, bool allowInvalid = false, bool? isWhite = null)
        {
            List<Point> result = new();

            int xp = p1.x == p2.x ? 0 : (p1.x < p2.x ? 1 : -1);
            int yp = p1.y == p2.y ? 0 : (p1.y < p2.y ? 1 : -1);


            if ((xp == 0 && yp == 0) || (xp != 0 && yp != 0 && Math.Abs(p1.x - p2.x) != Math.Abs(p1.y - p2.y)))
            {
                return allowInvalid ? InvalidQueryResult() : result;
            }

            int x = p1.x;
            int y = p1.y;

            if (forceInclusiveStart || board[x, y] == null)
            {
                result.Add(new Point(x, y));
            }
            x += xp;
            y += yp;

            while (p2.x != x || p2.y != y)
            {
                result.Add(new Point(x, y));
                x += xp;
                y += yp;
            }
            if (forceInclusiveEnd || board[p2] == null || (board[p2] != null && board[p1] != null && (isWhite == null ? (board[p1]!.IsWhite != board[p2]!.IsWhite) : (bool)isWhite)))
            {
                result.Add(new Point(x, y));
            }

            return Utils.FilterPoints(result);
        }

        public static List<Point> DrawLane(Point p1, Point p2, bool allowInvalid = true)
        {
            List<Point> result = new();

            int xp = p1.x == p2.x ? 0 : (p1.x < p2.x ? 1 : -1);
            int yp = p1.y == p2.y ? 0 : (p1.y < p2.y ? 1 : -1);


            if ((xp == 0 && yp == 0) || (xp != 0 && yp != 0 && Math.Abs(p1.x - p2.x) != Math.Abs(p1.y - p2.y)))
            {
                return allowInvalid ? InvalidQueryResult() : result;
            }

            int x = p1.x;
            int y = p1.y;
            

            while (x > 0 && x < 7 && y > 0 && y < 7)
            {
                x -= xp;
                y -= yp;
            }

            while ((xp == 1 ? (x < 8) : (x > -1)) && (yp == 1 ? (y < 8) : (y > -1)))
            {
                result.Add(new Point(x, y));
                x += xp;
                y += yp;
            }


            return Utils.FilterPoints(result);
        }

        public static List<Point> InvalidQueryResult()
        {
            List<Point> result = new();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    result.Add(new Point(i, j));
                }
            }
            return result;
        }

        public bool Remove(Point p)
        {
            return Remove(this[p]!);
        }
        public bool Remove(ChessPiece p)
        {
            if (p == null)
            {
                return false;
            }
            try
            {
                if (IsMain)
                {
                    Button button = (Button)MainWindow._instance.GetPieceByGridIdx(p.CurrentPosition!.y, p.CurrentPosition.x);
                    Grid buttonGrid = button.Content as Grid;
                    buttonGrid.Children.Clear();
                }
                Pieces.Remove(p);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Clear()
        {
            Pieces.Clear();
            LegalMoves.Clear();
            selectedPiece = null;
        }

        public static bool IsPointLegal(Point p)
        {
            return p.x > -1 && p.x < 8 && p.y > -1 && p.y < 8;
        }

        public static List<Point> FilterPoints(List<Point> pointsList)
        {
            return pointsList.Where(x => IsPointLegal(x)).ToList();
        }

        public ChessPiece? this[Point p]
        {
            get
            {
                return Pieces.Find(x => x.CurrentPosition! == p);
            }
            set
            {
                if (value == null || !IsPointLegal(value.CurrentPosition!)) return;
                
                int findIndex = Pieces.FindIndex(x => x.CurrentPosition! == p);
                
                if (findIndex != -1)
                {
                    Pieces.RemoveAt(findIndex);
                }
                value.CurrentPosition = p;
                if (!Pieces.Contains(value))
                {
                    Pieces.Add(value!);
                }
                
            }
        }

        public ChessPiece? this[int x, int y]
        {
            get
            {
                return this[new Point(x, y)];
            }
            set
            {
                this[new Point(x, y)] = value;
            }
        }

        public List<ChessPiece>.Enumerator GetEnumerator()
        {
            return Pieces.GetEnumerator();
        }

        public Board()
        {
            SetDefaultChessPosition();
        }

        public Board(List<ChessPiece> startingPosition)
        {
            Pieces = startingPosition.Where(x=> x!=null).ToList();
        }

        public Board(List<ChessPiece> startingPosition, bool isMain)
        {
            Pieces = startingPosition.Where(x => x != null).ToList();
            IsMain = isMain;
        }

        public void SetDefaultChessPosition()
        {
            //Pieces.Clear();
            //Pieces.Add(new Knight(new Point(1, 2), true));
            //Pieces.Add(new Bishop(new Point(4, 4), false));
            //Pieces.Add(new King(new Point(1, 1), true));
            //Pieces.Add(new King(new Point(7, 0), false));
            //return;

            Pieces.Clear();
            for (int i = 0; i < 8; i++)
            {
                Pieces.Add(new Pawn(new Point(i, 6), true));
                Pieces.Add(new Pawn(new Point(i, 1), false));
            }
            Pieces.Add(new Rook(new Point(7, 7), true));
            Pieces.Add(new Rook(new Point(0, 7), true));
            Pieces.Add(new Rook(new Point(7, 0), false));
            Pieces.Add(new Rook(new Point(0, 0), false));
            Pieces.Add(new Knight(new Point(6, 7), true));
            Pieces.Add(new Knight(new Point(1, 7), true));
            Pieces.Add(new Knight(new Point(6, 0), false));
            Pieces.Add(new Knight(new Point(1, 0), false));
            Pieces.Add(new Bishop(new Point(5, 7), true));
            Pieces.Add(new Bishop(new Point(2, 7), true));
            Pieces.Add(new Bishop(new Point(5, 0), false));
            Pieces.Add(new Bishop(new Point(2, 0), false));
            Pieces.Add(new King(new Point(4, 7), true));
            Pieces.Add(new King(new Point(4, 0), false));
            Pieces.Add(new Queen(new Point(3, 7), true));
            Pieces.Add(new Queen(new Point(3, 0), false));
        }
            
    }

    //- class Board
    //- - ** Grants the ability to query positions with both Point and matrix operands**
    //- - ChessPiece this[point]
    //- - ChessPiece this[x, y]
    //- - bool Contains(p)
    //- - bool Contains(x,y)
    //- - static bool IsPointLegal(p) => return 0<=X,Y<=8
    //- - static bool FilterPoints(pointList) => pointList.Where(p => IsPointLegal(p))
    //- -void SetStartingPosition() => Sets default start position
    //- - void SetStartingPosition(chessPieceList) => For the editor
}
