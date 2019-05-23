public class RightStrategy : IGameStrategy
{
    public void DoPair (int[,] matrix, Cell cell1, Cell cell2)
    {
        int firstMoveCol = cell1.col;
        int secondMoveCol = cell2.col;
        if (cell1.row == cell2.row)
        {
            firstMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
            secondMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
        }
        int cell1Row = cell1.row;
        for (int i = firstMoveCol; i > 0; i--)
        {
            matrix[cell1Row, i] = matrix[cell1Row, i - 1];
        }
        matrix[cell1Row, 0] = 0;
        int cell2Row = cell2.row;
        for (int i = secondMoveCol; i > 0; i--)
        {
            matrix[cell2Row, i] = matrix[cell2Row, i - 1];
        }
        matrix[cell2Row, 0] = 0;
    }
}