using UnityEngine;
using System.Collections;

public class BaseUtil {
    public static int GetCellId (Cell cell, int rowNum, int colNum)
    {
        return colNum * (cell.row - 1) + cell.col;
    }
    public static Cell GetCellById (int id, int rowNum, int colNum)
    {
        return new Cell (id/colNum + 1, id % colNum);
    }
}
