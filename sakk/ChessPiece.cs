﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace sakk
{
    public abstract partial class ChessPiece
    {
        public Point? CurrentPosition { get; set; }
        public bool IsWhite { get; set; } = true;
        public abstract List<Point> GetMovesAll();
        
        public virtual List<Point> GetMovesAll(Board board)
        {
            return GetMovesAll();
        }
        public virtual List<Point> GetMovesAvailable(Board board)
        {
         
            return GetMovesAll(board);
        }
        public virtual List<Point> GetMovesPinned(Board board)
        {
            return GetMovesAvailable(board);
        }
        public virtual List<Point> GetMovesFinal(Board board)
        {
            return GetMovesPinned(board);
        }
        private static BitmapImage[] bitmapImages = new BitmapImage[]
        {
            new BitmapImage(new Uri("pack://application:,,,/images/br.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/bn.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/bb.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/bq.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/bk.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/bp.png")),

            new BitmapImage(new Uri("pack://application:,,,/images/wr.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/wn.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/wb.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/wq.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/wk.png")),
            new BitmapImage(new Uri("pack://application:,,,/images/wp.png")),

        };

        public BitmapImage ImageByIdx
        {
            get
            {
                return bitmapImages[imageIdx + (IsWhite ? 6 : 0)];
            }
        }
        public abstract int imageIdx { get; set; }

    }


}