using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace sakk
{
    public abstract partial class ChessPiece
    {
        public Point? CurrentPosition { get; set; }
        public bool IsWhite { get; set; } = true;

        public List<Point> GetAttackers(Board board)
        {

            return Board.QueenEndpoints(board, CurrentPosition!)
                .Where(p => board[p] != null && (board[p] is Pawn ? (board[p]!
                .GetMovesDefending(board).Any(x => x == CurrentPosition!)) : (board[p]!
                .GetMovesAvailable(board).Any(x => x == CurrentPosition!))))
                .ToList();
        }
        public bool HasAttacker(Board board)
        {

            return GetAttackers(board).Count != 0;
        }

        public List<Point> GetAttackers(Board board, Point point, bool? isWhite)
        {

            return Board.QueenEndpoints(board, point, ignoreColor: true)
                .Where(
                    ap => board[ap] != null
                    && (isWhite == null ? true : (board[ap].IsWhite != (bool)isWhite))
                    && (board[ap] is Pawn ?
                        (board[ap]!
                        .GetMovesDefending(board).Any(move => move == point!))
                        :
                        (board[ap] is King ? (board[ap]!.GetMovesAll(board).Any(move => move == point!)) : (board[ap]!
                        .GetMovesAvailable(board).Any(move => move == point!)))))
                .ToList();
        }

        public bool HasDefender(Board board)
        {

            return Board.QueenEndpoints(board, this.CurrentPosition, ignoreColor: true)
                .Where(
                    p => board[p] != null
                    && (board[p].IsWhite == this.IsWhite)
                    && board[p]!
                    .GetMovesDefending(board)
                    .Any(x => x == this.CurrentPosition))
                .ToList().Any();
        }


        public bool HasDefenderNull(Board board, Point position, bool IsWhite)
        {
            return Board.QueenEndpoints(board, this.CurrentPosition, ignoreColor: true)
                .Where(
                    p => board[p] != null
                    && (board[p].IsWhite == this.IsWhite)
                    && board[p]!
                    .GetMovesDefending(board)
                    .Any(x => x == this.CurrentPosition))
                .ToList().Any();
        }
        public bool HasAttacker(Board board, Point point, bool? isWhite)
        {
            return GetAttackers(board, point, isWhite).Count != 0;
        }
        public abstract List<Point> GetMovesAll();
        
        public virtual List<Point> GetMovesAll(Board board)
        {
            return GetMovesAll();
        }

        public virtual List<Point> GetMovesAllSelfInclusive()
        {
            var temp = GetMovesAll();
            temp.Add(new(CurrentPosition!.x, CurrentPosition.y));
            return temp;
        }
        public virtual List<Point> GetMovesAvailable(Board board)
        {
         
            return GetMovesAll(board);
        }

        public abstract List<Point> GetMovesDefending(Board board);

        public virtual List<Point> GetMovesPinned(Board board)
        {
            List<Point> kingLine = Board.DrawLane(CurrentPosition, board.FindKingPoint(IsWhite));
            bool isObstructed = false;
            bool runChecks = false;
            foreach (var p in kingLine)
            {
                if (runChecks)
                {
                    if (board[p] != null)
                    {
                        isObstructed = board[p] is not King;
                        break;
                    }
                }
                else
                {
                    runChecks = p == CurrentPosition;
                }
            }
            var availableMoves = GetMovesAvailable(board);
            if (isObstructed)
            {
                return availableMoves;
            }
            foreach (Point attackerPoint in GetAttackers(board))
            {
                var attackerMoves = board[attackerPoint]!.GetMovesAllSelfInclusive();
                if (Utils.IsPointSetsEqual(kingLine, Utils.PointsAnd(attackerMoves, kingLine)))
                {
                    List<Point> midSection = Board.DrawSection(board, board[attackerPoint]!.CurrentPosition, board.FindKingPoint(IsWhite), forceInclusiveStart: true);
                    int findIdx = midSection.IndexOf(CurrentPosition);
                    if (findIdx != -1)
                    {
                        availableMoves = Utils.PointsAnd(midSection, availableMoves);
                        break;
                    }
                }
            }
            return availableMoves;
        }
        public virtual List<Point> GetMovesFinal(Board board)
        {
            var attackerPoints = GetAttackers(board, board.FindKingPoint(IsWhite), IsWhite);
            switch (attackerPoints.Count)
            {
                case 0:
                    return GetMovesPinned(board);
                case 1:
                    return Utils.PointsAnd(GetMovesPinned(board), Board.DrawSection(board, board[attackerPoints[0]]!.CurrentPosition!, board.FindKingPoint(IsWhite), forceInclusiveStart: true));
                default:
                    return new();
            }
        }

        public static BitmapImage[] bitmapImages = new BitmapImage[]
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
