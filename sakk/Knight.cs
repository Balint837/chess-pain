using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sakk
{
    internal class Knight : ChessPiece
    {
        public Knight(Point position, bool isWhite)
        {
            this.CurrentPosition = position;
            this.IsWhite = isWhite;
        }

        public override int imageIdx { get; set; } = 1;

        public override List<Point> GetMovesAll()
        {

            List<Point> result = new List<Point>();
            result.Add(new Point(CurrentPosition.x + 1, CurrentPosition.y + 2));
            result.Add(new Point(CurrentPosition.x + 1, CurrentPosition.y - 2));
            result.Add(new Point(CurrentPosition.x - 1, CurrentPosition.y + 2));
            result.Add(new Point(CurrentPosition.x - 1, CurrentPosition.y - 2));
            result.Add(new Point(CurrentPosition.x + 2, CurrentPosition.y + 1));
            result.Add(new Point(CurrentPosition.x + 2, CurrentPosition.y - 1));
            result.Add(new Point(CurrentPosition.x - 2, CurrentPosition.y + 1));
            result.Add(new Point(CurrentPosition.x - 2, CurrentPosition.y - 1));
                
            return Utils.FilterPoints(result);
            

        }

        public override List<Point> GetMovesAvailable(Board board)
        {
            return GetMovesAll().Where(p => board[p] == null || board[p].IsWhite != IsWhite).ToList();
        }

    }
}
