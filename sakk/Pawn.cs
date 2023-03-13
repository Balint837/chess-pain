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
        public bool mayBePassanted { get; set; } = false;
        public override int imageIdx { get; set; } = 5;

        public override List<Point> GetMovesAll()
        {
            List<Point> result = new List<Point>();
            if (IsWhite)
            {
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y - 1));
                if (IsFirstMove)
                {
                    result.Add(new Point(CurrentPosition.x, CurrentPosition.y - 2));
                }

                var checkPiece = MainWindow.board[new Point(CurrentPosition.x - 1, CurrentPosition.y - 1)];
                if (checkPiece != null && !checkPiece.IsWhite)
                {
                    result.Add(new Point(CurrentPosition.x - 1, CurrentPosition.y - 1));
                }

                checkPiece = MainWindow.board[new Point(CurrentPosition.x + 1, CurrentPosition.y - 1)];
                if (checkPiece != null && !checkPiece.IsWhite)
                {
                    result.Add(new Point(CurrentPosition.x + 1, CurrentPosition.y - 1));
                }

            }
            else
            {
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y + 1));
                if (IsFirstMove)
                {
                    result.Add(new Point(CurrentPosition.x, CurrentPosition.y + 2));

                }


                var checkPiece = MainWindow.board[new Point(CurrentPosition.x - 1, CurrentPosition.y + 1)];
                if (checkPiece != null && checkPiece.IsWhite)
                {
                    result.Add(new Point(CurrentPosition.x - 1, CurrentPosition.y + 1));
                }

                checkPiece = MainWindow.board[new Point(CurrentPosition.x + 1, CurrentPosition.y + 1)];
                if (checkPiece != null && checkPiece.IsWhite)
                {
                    result.Add(new Point(CurrentPosition.x + 1, CurrentPosition.y + 1));
                }
            }
            var currentPoint = new Point(CurrentPosition.x + 1, CurrentPosition.y);
            var tempPiece = MainWindow.board[currentPoint];
            if (tempPiece is Pawn && ((Pawn)tempPiece).mayBePassanted)
            {
                currentPoint.y += IsWhite ? -1 : 1;
                result.Add(currentPoint);
            }


            currentPoint = new Point(CurrentPosition.x - 1, CurrentPosition.y);
            tempPiece = MainWindow.board[currentPoint];
            if (tempPiece is Pawn && ((Pawn)tempPiece).mayBePassanted)
            {
                currentPoint.y += IsWhite ? -1 : 1;
                result.Add(currentPoint);
            }


            return Utils.FilterPoints(result);
        }

        public override List<Point> GetMovesAvailable(Board board)
        {
            List<Point> result = new List<Point>();
            int multiplier = IsWhite ? -1 : 1;
            Point checkPoint = new Point(CurrentPosition.x, CurrentPosition.y + 1 * multiplier);
            if (board[checkPoint] == null)
            {
                result.Add(checkPoint);
            }
            checkPoint = new Point(CurrentPosition.x, CurrentPosition.y + 2 * multiplier);
            if (IsFirstMove && board[checkPoint] == null)
            {
                result.Add(checkPoint);
            }

            var tempPiece = MainWindow.board[new Point(CurrentPosition.x - 1, CurrentPosition.y + 1 * multiplier)];
            if (tempPiece != null && tempPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x - 1, CurrentPosition.y + 1 * multiplier));
            }

            tempPiece = MainWindow.board[new Point(CurrentPosition.x + 1, CurrentPosition.y + 1 * multiplier)];
            if (tempPiece != null && tempPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x + 1, CurrentPosition.y + 1 * multiplier));
            }
            
            checkPoint = new Point(CurrentPosition.x + 1, CurrentPosition.y);
            tempPiece = MainWindow.board[checkPoint];
            if (tempPiece is Pawn && ((Pawn)tempPiece).mayBePassanted && tempPiece.IsWhite != IsWhite)
            {
                checkPoint.y += multiplier;
                result.Add(checkPoint);
            }

            checkPoint = new Point(CurrentPosition.x - 1, CurrentPosition.y);
            tempPiece = MainWindow.board[checkPoint];
            if (tempPiece is Pawn && ((Pawn)tempPiece).mayBePassanted && tempPiece.IsWhite != IsWhite)
            {
                checkPoint.y += multiplier;
                result.Add(checkPoint);
            }

            return Utils.FilterPoints(result);
        }
        //public override List<Point> GetPossibleMoves(Board board)
        //{

        //    {
        //        List<Point> result = new List<Point>();
        //        result = GetPossibleMoves();
        //        if (IsWhite)
        //        {
        //            if (board[CurrentPosition.x + 1, CurrentPosition.y] != null && board[CurrentPosition.x + 1, CurrentPosition.y].GetType == "Pawn")
        //            {
        //            }

        //        }

        //        return Utils.FilterPoints(result);
        //    }
        //}
    }
}
