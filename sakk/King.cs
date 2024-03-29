﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

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
            List<Point> moves = new();

            foreach (var p in GetMovesAll())
            {
                var currentPiece = board[p];
                if (currentPiece == null)
                {
                    Board b = new(board.Pieces.Select(x => (x.CurrentPosition == CurrentPosition) ? (new King(p, IsWhite) { IsFirstMove = false }) : (x.CurrentPosition == p ? null : x)).ToList(), false);
                    if (!HasAttacker(b, p, IsWhite))
                    {
                        moves.Add(p);
                    }
                }
                else if (currentPiece.IsWhite != IsWhite && !currentPiece.HasDefender(board))
                {
                    moves.Add(p);
                }
            }

            return moves;

        }

        public override List<Point> GetMovesDefending(Board board)
        {
            return GetMovesAll();
        }

        public override List<Point> GetMovesFinal(Board board)
        {
            var attackerPoints = GetAttackers(board, board.FindKingPoint(IsWhite), IsWhite);
            var result = GetMovesAvailable(board);
            switch (attackerPoints.Count)
            {
                case 0:
                    if (!IsFirstMove)
                    {
                        return result;
                    }
                    var row = IsWhite ? 7 : 0;
                    var leftPiece = board[0, row];
                    var rightPiece = board[7, row];
                    if (leftPiece != null && leftPiece is Rook)
                    {
                        Rook leftRook = (Rook)leftPiece;
                        if (leftRook.IsFirstMove)
                        {
                            var tp1 = new Point(3, row);
                            var tp2 = new Point(2, row);
                            var tp3 = new Point(1, row);
                            if (result.Any(p => p == tp1) && board[tp2] == null && board[tp3] == null && !HasAttacker(board, tp2, IsWhite))
                            {
                                result.Add(tp2);
                            }
                        }
                    }
                    if (rightPiece != null && rightPiece is Rook)
                    {
                        Rook rightRook = (Rook)rightPiece;
                        if (rightRook.IsFirstMove)
                        {
                            var tp1 = new Point(5, row);
                            var tp2 = new Point(6, row);
                            if (result.Any(p => p == tp1) && board[tp2] == null && !HasAttacker(board, tp2, IsWhite))
                            {
                                result.Add(tp2);
                            }
                        }
                    }
                    return result;
                default:
                    return result;
            }
        }

    }
}
