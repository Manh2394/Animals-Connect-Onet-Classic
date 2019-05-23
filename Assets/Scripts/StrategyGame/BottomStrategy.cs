public class BottomStrategy : IGameStrategy
{
    public void DoPair (int[,] matrix, Cell cell1, Cell cell2)
    {
        matrix[cell1.row, cell1.col] = 0;
        matrix[cell2.row, cell2.col] = 0;

        int firstMoveRow = cell1.row;
        int secondMoveRow = cell2.row;
        if (cell1.col == cell2.col)
        {
            firstMoveRow = cell1.row > cell2.row ? cell1.row : cell2.row;
            secondMoveRow = cell1.row > cell2.row ? cell2.row : cell1.row;
        }
        int cell1Col = cell1.col;
        for (int i = firstMoveRow; i < matrix.GetLength (0) - 2; i++)
        {
            matrix[i, cell1Col] = matrix[i + 1, cell1Col];
        }
        matrix[matrix.GetLength (0) - 2, cell1.col] = 0;
        int cell2Col = cell2.col;
        for (int i = secondMoveRow; i < matrix.GetLength (0) - 2; i++)
        {
            matrix[i, cell2Col] = matrix[i + 1, cell2Col];
        }
        matrix[matrix.GetLength (0) - 2, cell2.col] = 0;
    }
}