using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sakk
{
    internal class Queen: ChessPiece
    {
        public Queen(Point position, bool isWhite)
        {
            this.CurrentPosition = position;
            this.IsWhite = isWhite;
        }

        public override int imageIdx { get; set; } = 3;

        public override List<Point> GetMovesAll()
        {
            List<Point> result = new List<Point>();
            for (int i = 1; i < 8; i++)
            {
                result.Add(new Point(CurrentPosition.x + i, CurrentPosition.y));
                result.Add(new Point(CurrentPosition.x - i, CurrentPosition.y));
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y + i));
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y - i));
                result.Add(new Point(CurrentPosition.x + i, CurrentPosition.y + i));
                result.Add(new Point(CurrentPosition.x - i, CurrentPosition.y + i));
                result.Add(new Point(CurrentPosition.x + i, CurrentPosition.y - i));
                result.Add(new Point(CurrentPosition.x - i, CurrentPosition.y - i));
            }
            return Utils.FilterPoints(result);
        }



        public override List<Point> GetMovesAvailable(Board board)
        {
            List<Point> result = new List<Point>();
            foreach (Point p in Board.QueenEndpoints(CurrentPosition!))
            {
                result.AddRange(Board.DrawSection(CurrentPosition!, p));
            };

            return Utils.FilterPoints(result);
        }


        public override List<Point> GetMovesDefending(Board board)
        {
            List<Point> result = new List<Point>();
            foreach (Point p in Board.QueenEndpoints(CurrentPosition!))
            {
                result.AddRange(Board.DrawSection(CurrentPosition!, p, forceInclusiveEnd: true));
            };

            return Utils.FilterPoints(result);
        }
    }
}
