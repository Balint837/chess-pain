﻿using System;
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



        public override List<Point> GetMovesAvailable(Board board)
        {
            List<Point> result = new List<Point>();
            int iter = 1;
            int maxIter = CurrentPosition.x > CurrentPosition.y ? 8-CurrentPosition.x : 8-CurrentPosition.y;
            while (board[new Point(CurrentPosition.x + iter, CurrentPosition.y + iter)] == null && iter < maxIter)
            {
                result.Add(new Point(CurrentPosition.x + iter, CurrentPosition.y + iter));
                iter++;
            }
            var lastPiece = board[new Point(CurrentPosition.x + iter, CurrentPosition.y + iter)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x + iter, CurrentPosition.y + iter));
            }


            iter = 1;
            maxIter = CurrentPosition.x > CurrentPosition.y ? CurrentPosition.y + 1 : CurrentPosition.x + 1;
            while (board[new Point(CurrentPosition.x - iter, CurrentPosition.y - iter)] == null && iter < maxIter)
            {
                result.Add(new Point(CurrentPosition.x - iter, CurrentPosition.y - iter));
                iter++;
            }
            lastPiece = board[new Point(CurrentPosition.x - iter, CurrentPosition.y - iter)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x - iter, CurrentPosition.y - iter));
            }



            iter = 1;
            maxIter = CurrentPosition.x+1 > 8-CurrentPosition.y ? 8-CurrentPosition.y : CurrentPosition.x+1;
            while (board[new Point(CurrentPosition.x - iter, CurrentPosition.y + iter)] == null && iter < maxIter)
            {
                result.Add(new Point(CurrentPosition.x - iter, CurrentPosition.y + iter));
                iter++;
            }
            lastPiece = board[new Point(CurrentPosition.x - iter, CurrentPosition.y + iter)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x - iter, CurrentPosition.y + iter));
            }


            iter = 1;
            maxIter = 8 - CurrentPosition.x > CurrentPosition.y + 1 ? CurrentPosition.y + 1 : 8 - CurrentPosition.x;
            while (board[new Point(CurrentPosition.x + iter, CurrentPosition.y - iter)] == null && iter < maxIter)
            {
                result.Add(new Point(CurrentPosition.x + iter, CurrentPosition.y - iter));
                iter++;
            }
            lastPiece = board[new Point(CurrentPosition.x + iter, CurrentPosition.y - iter)];

            if (lastPiece != null && lastPiece.IsWhite != IsWhite)
            {
                result.Add(new Point(CurrentPosition.x + iter, CurrentPosition.y - iter));
            }

            return Utils.FilterPoints(result);
            
        }

    }
}