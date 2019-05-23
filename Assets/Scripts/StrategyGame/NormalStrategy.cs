public class NormalStrategy : IGameStrategy
{
    public void DoPair (int[,] matrix, Cell cell1, Cell cell2)
    {
        matrix[cell1.row, cell1.col] = 0;
        matrix[cell2.row, cell2.col] = 0;
        int cell1Id = BaseUtil.GetCellId (cell1, matrix.GetLength (0), matrix.GetLength (1));
        int cell2Id = BaseUtil.GetCellId (cell2, matrix.GetLength (0), matrix.GetLength (1));
    }
}