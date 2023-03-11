﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace sakk
{
    public class Rook : ChessPiece
    {

        public Rook(Point position, bool isWhite)
        {
            this.CurrentPosition = position;
            this.IsWhite = isWhite;
        }
        public Rook(Point position, bool isWhite, bool isFirstMove)
        {
            this.CurrentPosition = position;
            this.IsWhite = isWhite;
            IsFirstMove = isFirstMove;
        }

        public override int imageIdx { get; set; } = 0;

        public bool IsFirstMove { get; set; } = true;


        public override List<Point> GetMovesAll()
        {
            List<Point> result = new List<Point>();
            for (int i = 1; i < 8; i++)
            {
                result.Add(new Point(CurrentPosition.x + i, CurrentPosition.y));
                result.Add(new Point(CurrentPosition.x - i, CurrentPosition.y));
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y + i));
                result.Add(new Point(CurrentPosition.x, CurrentPosition.y - i));
            }
            return Utils.FilterPoints(result);
            
        }
        
    }
}
