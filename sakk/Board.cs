using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace sakk
{
    public class Board
    {
        List<ChessPiece> Pieces = new List<ChessPiece>();
        public List<Point> LegalMoves = new List<Point>();
        public ChessPiece selectedPiece;
        
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
            Pieces = startingPosition;
        }


        public void SetDefaultChessPosition()
        {

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
