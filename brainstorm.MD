# Classes
- class MainWindow
- - ChessPiece[8,8] pieces

- class Utils
- - bool[8,8] QueenMoves(x,y)
- - bool[8,8] HorseMoves(x,y)
- - bool[8,8] MatrixOr(matrix1, matrix2)
- - bool[8,8] MatrixAnd(matrix1, matrix2)
- - bool[8,8] MatrixXor(matrix1, matrix2)

- abstract class ChessPiece
- - static bool IsKingInCheck
- - abstract bool[8,8] GetMovesAll
- - abstract bool[8,8] GetMovesAvailable (Handles IsKingInCheck & Pin)
- - bool isBlack
- - abstract static BitmapImage image

- class Pawn
- - HasMoved
- -

- class Rook
- - HasMoved

- class Knight

- 