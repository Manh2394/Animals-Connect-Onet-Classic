public class LeftAndRightStrategy : IGameStrategy
{
    public void DoPair (int[,] matrix, Cell cell1, Cell cell2)
    {
        int firstMoveCol = cell1.col;
        int secondMoveCol = cell2.col;
        int rowNum = matrix.GetLength (0);
        int colNum = matrix.GetLength (1);
        if (cell1.row == cell2.row && cell1.col < (colNum-2)/2 && cell2.col < (colNum-2)/2)
        {
            firstMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
            secondMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
        }

        if (cell1.row == cell2.row && cell1.col > (colNum-2)/2 && cell2.col > (colNum-2)/2)
        {
            firstMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
            secondMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
        }

        int cell1Row = cell1.row;
        if (cell1.col <= (colNum -2)/2)
        {
            for (int i = firstMoveCol; i < (colNum - 2)/2; i++)
            {
                matrix[cell1Row, i] = matrix[cell1Row, i + 1];
            }
            matrix[cell1Row,  (colNum - 2)/2] = 0;
            
        } 
        else
        {
            for (int i = firstMoveCol; i > (colNum -2)/2; i--)
            {
                matrix[cell1Row, i] = matrix[cell1Row, i - 1];
            }
            matrix[cell1Row,  (colNum - 2)/2 + 1] = 0;

        }
        int cell2Row = cell2.row;
        if (cell2.col <= (colNum -2)/2)
        {
            for (int i = secondMoveCol; i < (colNum - 2)/2; i++)
            {
                matrix[cell2Row, i] = matrix[cell2Row, i + 1];
            }
            matrix[cell2Row,  (colNum - 2)/2] = 0;            
        } else
        {
            for (int i = secondMoveCol; i > (colNum -2)/2 + 1; i--)
            {
                matrix[cell2Row, i] = matrix[cell2Row, i - 1];
            }
            matrix[cell2Row,  (colNum - 2)/2 + 1] = 0;
        }

    }
}