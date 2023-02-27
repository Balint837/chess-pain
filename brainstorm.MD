# Classes
**Giving a \* to a parameter means that it's optional.'**
- class MainWindow
- - Board pieces



- static class Utils
- - static List<Point> PointsOr(matrix1, matrix2)
- - static List<Point> PointsAnd(matrix1, matrix2)



- class Point
- - int X
- - int Y
- - operator -(p1, p2)
- - operator +(p1, p2)
- - bool IsEqual(p) => whether X,Y of instance is equal to X,Y of p



- class Board
- - **Grants the ability to query positions with both Point and matrix operands**
- - ChessPiece this[point]
- - ChessPiece this[x,y]
- - bool Contains(p)
- - bool Contains(x,y)
- - static bool IsPointLegal(p) => return 0<=X,Y<=8
- - static bool FilterPoints(pointList) => pointList.Where(p => IsPointLegal(p))
- - void SetStartingPosition() => Sets default start position
- - void SetStartingPosition(chessPieceList) => For the editor




- abstract class ChessPiece
- - static List<Point> FindKingLine(x,y,isBlack*,opposing*) => Find king with queen moves => returns whole diagonal/row/column, or whole board if not found (opposing* acts as partial* as well)
- - static List<Point> QueenMoves(x,y,isBlack*) => Queen moves that don't go over friendlies
- - static List<Point> HorseMoves(x,y,isBlack*) => Horse moves that don't capture friendlies ()
- - static List<Point> GetAttackers(x,y,isBlack*) => Find enemy endpoints with queen moves that can attack current position
- - static Point GetAttacker(x,y,isBlack*) => Finds the first enemy with queen moves that can attack current position
- - static List<Point> GetAttackersCount(x,y,isBlack*) => Shorthand for MatrixCount(GetAttackers(x,y,isBlack))
- - static List<Point> HasAttacker(x,y,isBlack*) => Shorthand for MatrixAny(GetAttackers(x,y,isBlack))
- - static List<Point> AvailablePieces(x,y,isBlack*) => Returns matrix of available pieces
- - static Point FindKing(isBlack*) => returns king position
- - **Shorthand for the above: Function(chessPiece)**

- - abstract List<Point> GetMovesAll() => Moves of piece that are defined
- - virtual List<Point> GetMovesAvailable() => Moves of piece that don't go over friendlies (made abstract instead of virtual due to algorithmic problems)
- - virtual List<Point> GetMovesPinned()
- - - Run MatrixAnd for each piece in GetAttackers on Attacker.GetMovesAll and FindKingLine.
- - - If there is an instance where the result is equal to original FindKingLine, run MatrixAnd(Attacker.GetMovesAvailable, Piece.GetMovesAvailable)
- - virtual List<Point> GetMovesFinal() => if (HasAttacker(FindKing()) == false) then same as GetMovesPinned else if (MatrixCount(GetAttackers(FindKing())) > 1) then no moves else GetAttackers(FindKing())

- - bool isBlack

- - pont

- - abstract static BitmapImage image



- class Pawn
- - bool HasMoved
- - bool MayBePassanted

- class Rook
- - bool HasMoved

- class Knight

- class Bishop

- class Queen

- class King
- - bool HasMoved
- - int CheckedCount