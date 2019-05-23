public class YCenterStrategy : IGameStrategy
{
    public void DoPair (int[,] matrix, Cell cell1, Cell cell2)
    {
        matrix[cell1.row, cell1.col] = 0;
        matrix[cell2.row, cell2.col] = 0;
        int rowNum = matrix.GetLength (0);
        int colNum = matrix.GetLength (1);

        int firstMoveRow = cell1.row;
        int secondMoveRow = cell2.row;
        if (cell1.col == cell2.col && cell1.row <= (rowNum-2)/2 && cell2.row <= (rowNum-2)/2)
        {
            firstMoveRow = cell1.row > cell2.row ? cell2.row : cell1.row;
            secondMoveRow = cell1.row > cell2.row ? cell1.row : cell2.row;
        }

        if (cell1.col == cell2.col && cell1.row > (rowNum-2)/2 && cell2.row > (rowNum-2)/2)
        {
            firstMoveRow = cell1.row > cell2.row ? cell1.row : cell2.row;
            secondMoveRow = cell1.row > cell2.row ? cell2.row : cell1.row;
        }

        int cell1Col = cell1.col;
        if (cell1.row <= (rowNum -2)/2)
        {
            for (int i = firstMoveRow; i > 0; i--)
            {
                matrix[i, cell1Col] = matrix[i - 1, cell1Col];
            }
        } else
        {
            for (int i = firstMoveRow; i < (rowNum - 2); i++)
            {
                matrix[i, cell1Col] = matrix[i + 1, cell1Col];
            }     
            matrix[rowNum - 2, cell1.col] = 0;            
       
        }

        int cell2Col = cell2.col;
        if (cell2.row <= (rowNum -2)/2)
        {
            for (int i = secondMoveRow; i > 0; i--)
            {
                matrix[i, cell2Col] = matrix[i - 1, cell2Col];
            }
        } else
        {
            for (int i = secondMoveRow; i < (rowNum - 2); i++)
            {
                matrix[i, cell2Col] = matrix[i + 1, cell2Col];
            }     
            matrix[rowNum - 2, cell2.col] = 0;            
       
        }

    }
}