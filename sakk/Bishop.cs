using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sakk
{
    internal class Bishop : ChessPiece
    {

        public Bishop(Point position, bool isWhite)
        {
            this.CurrentPosition = position;
            this.IsWhite = isWhite;
        }

        public override int imageIdx { get; set; } = 2;

        public override List<Point> GetMovesAll()
        {
            List<Point> result = new List<Point>();
            for (int i = 1; i < 8; i++)
            {
                result.Add(new Point(CurrentPosition.x + i, CurrentPosition.y + i));
                result.Add(new Point(CurrentPosition.x - i, CurrentPosition.y + i));
                result.Add(new Point(CurrentPosition.x + i, CurrentPosition.y - i));
                result.Add(new Point(CurrentPosition.x - i, CurrentPosition.y - i));
            }
            return Utils.FilterPoints(result);
        }

    }
}
