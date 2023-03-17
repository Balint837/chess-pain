﻿using System;
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
            int iter = CurrentPosition.y + 1;
            while (board[new Point(CurrentPosition.x, iter)] == null && iter < 8)
            {
                result.Add(new Point(CurrentPosition.x, iter));
                iter++;
            }
            var lastPiece = board[new Point(CurrentPosition.x, iter)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x, iter));
            }

            iter = CurrentPosition.y - 1;
            while (board[new Point(CurrentPosition.x, iter)] == null && iter > -1)
            {
                result.Add(new Point(CurrentPosition.x, iter));
                iter--;
            }
            lastPiece = board[new Point(CurrentPosition.x, iter)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x, iter));
            }



            iter = CurrentPosition.x + 1;
            while (board[new Point(iter, CurrentPosition.y)] == null && iter < 8)
            {
                result.Add(new Point(iter, CurrentPosition.y));
                iter++;
            }
            lastPiece = board[new Point(iter, CurrentPosition.y)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(iter, CurrentPosition.y));
            }



            iter = CurrentPosition.x - 1;
            while (board[new Point(iter, CurrentPosition.y)] == null && iter > -1)
            {
                result.Add(new Point(iter, CurrentPosition.y));
                iter--;
            }
            lastPiece = board[new Point(iter, CurrentPosition.y)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(iter, CurrentPosition.y));
            }

            return Utils.FilterPoints(result);
        }
    }
}