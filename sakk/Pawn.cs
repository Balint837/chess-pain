using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sakk
{
    internal class Pawn: ChessPiece
    {
        public Pawn(Point position, bool isWhite)
        {
            this.CurrentPosition = position;
            this.IsWhite = isWhite;
        }
        public bool IsFirstMove { get; set; } = true;

        public override int imageIdx { get; set; } = 5;

        public override List<Point> GetPossibleMoves()
        {
            List<Point> result = new List<Point>();
            if (IsWhite)
            {
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y - 1));
                if (IsFirstMove)
                {
                    result.Add(new Point(CurrentPosition.x, CurrentPosition.y - 2));
                }
                
            }
            else
            {
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y + 1));
                if (IsFirstMove)
                {
                    result.Add(new Point(CurrentPosition.x, CurrentPosition.y + 2));

                }
            }
            
            return Utils.FilterPoints(result);
        }

    }
}
