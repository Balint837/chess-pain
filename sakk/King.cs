using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sakk
{
    internal class King : ChessPiece
    {
        public King(Point position, bool isWhite)
        {
            this.CurrentPosition = position;
            this.IsWhite = isWhite;
        }

        public override int imageIdx { get; set; } = 4;
        public bool IsFirstMove { get; set; } = true;

        public override List<Point> GetMovesAll()
        {
            List<Point> result = new List<Point>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)
                    {
                            
                        result.Add(new Point(CurrentPosition.x + i, CurrentPosition.y + j));
                    }
                    
                }
            }
            return Utils.FilterPoints(result);
        }

        public override List<Point> GetMovesAvailable(Board board)
        {
            return GetMovesAll().Where(p => (board[p] == null || board[p].IsWhite != IsWhite) && !HasAttacker(board, p, IsWhite)).ToList();
        }

    }
}
