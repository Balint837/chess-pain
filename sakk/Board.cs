using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace sakk
{
    public class Board
    {
        List<ChessPiece> Pieces = new List<ChessPiece>();

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
                int findIndex = Pieces.FindIndex(x => x.CurrentPosition! == value!.CurrentPosition!);
                if (findIndex != -1)
                {
                    Pieces.RemoveAt(findIndex);
                }
                Pieces.Add(value!);
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
