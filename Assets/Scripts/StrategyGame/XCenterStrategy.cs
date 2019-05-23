public class XCenterStrategy : IGameStrategy
{
    public void DoPair (int[,] matrix, Cell cell1, Cell cell2)
    {
        matrix[cell1.row, cell1.col] = 0;
        matrix[cell2.row, cell2.col] = 0;
        int rowNum = matrix.GetLength (0);
        int colNum = matrix.GetLength (1);

        int firstMoveCol = cell1.col;
        int secondMoveCol = cell2.col;
        if (cell1.row == cell2.row && cell1.col <= (colNum-2)/2 && cell2.col <= (colNum-2)/2)
        {
            firstMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
            secondMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
        }

        if (cell1.row == cell2.row && cell1.col > (colNum-2)/2 && cell2.col > (colNum-2)/2)
        {
            firstMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
            secondMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
        }

        int cell1Row = cell1.row;
        if (cell1.col <= (colNum -2)/2)
        {
            for (int i = firstMoveCol; i > 0; i--)
            {
                matrix[cell1Row, i] = matrix[cell1Row, i-1];
            }
        } else
        {
            for (int i = firstMoveCol; i < colNum-2; i++)
            {
                matrix[cell1Row, i] = matrix[cell1Row, i+1];
            }       
            matrix[cell1Row, colNum -2] = 0;
        }

        int cell2Row = cell2.row;
        if (cell2.col <= (colNum -2)/2)
        {
            for (int i = secondMoveCol; i > 0; i--)
            {
                matrix[cell2Row, i] = matrix[cell2Row, i-1];
            }
        } else
        {
            for (int i = secondMoveCol; i < colNum-2; i++)
            {
                matrix[cell2Row, i] = matrix[cell2Row, i+1];
            }     
            matrix[cell2Row, colNum -2] = 0;
       
        }

    }
}