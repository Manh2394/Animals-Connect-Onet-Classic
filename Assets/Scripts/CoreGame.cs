using UnityEngine;
using Assets.Phunk.Core;
using System;
using System.Collections.Generic;

public class CoreGame
{

    public static Dictionary<int, int> GetAvailablePair (int[,] matrix)
    {
        Dictionary<int, int> result = new Dictionary<int, int> ();
        int row = matrix.GetLength (0);
        int col = matrix.GetLength (1);

        List<int> pairedCellIds = new List<int> ();
        float startTime = Time.realtimeSinceStartup;
        for (int i = 1; i < row -1; i++)
        {
            for (int j = 1; j < col -1; j++)
            {
                Cell p1 = new Cell (i, j);
                if (pairedCellIds.Contains (BaseUtil.GetCellId (p1, row, col)) || matrix[p1.row, p1.col] == 0)
                    continue;
                for (int x = 1; x < row -1; x++)
                {
                    for (int y = 1; y < col -1; y++)
                    {
                        if (x < i && y < j)
                            continue;
                        Cell p2 = new Cell (x, y);
                        if (pairedCellIds.Contains (BaseUtil.GetCellId (p2, row, col)) || matrix[p2.row, p2.col] == 0)
                            continue;
                        if (CoreGame.GetWayBetweenTwoCell (matrix, p1, p2).Count > 0) {
                            x = i = row;
                            y = j = col;
                            pairedCellIds.Add (BaseUtil.GetCellId (p1, row, col));
                            pairedCellIds.Add (BaseUtil.GetCellId (p2, row, col));
                            x = row -1;
                            y = col -1;
                            result.Add (BaseUtil.GetCellId (p1, row, col), BaseUtil.GetCellId (p2, row, col));
                            Log.Warning ("match: cell" + p1.ToString () + " vs " + p2.ToString ());
                        }
                    }
                }
            }
        }
        return result;
    }

	public static int[,] CreateMatrix(int row, int col) {
        int[,] matrix = new int[row, col];
		for (int y = 0; y < col; y++) {
			matrix[0, y] = matrix[row - 1, y] = 0;
		}
		for (int x = 0; x < row; x++) {
			matrix[x, 0] = matrix[x, col - 1] = 0;
		}

		int imgNumber = Consts.pokemonNum;
		int maxDouble = imgNumber / 4;
		int[] imgArr = new int[imgNumber + 1];
		List<Cell> cells = new List<Cell>();
		for (int x = 1; x < row - 1; x++) {
			for (int y = 1; y < col - 1; y++) {
                cells.Add (new Cell(x, y));
			}
		}

		int i = 0;
		do {
			int imgIndex = UnityEngine.Random.Range (1, imgNumber - 1);
            if (imgArr[imgIndex] < maxDouble) {
				imgArr[imgIndex] += 2;
				for (int j = 0; j < 2; j++) {
					try {
						int size = cells.Count;
						int pointIndex = UnityEngine.Random.Range (0, size);
                        int cellRow = cells[pointIndex].row;
                        int cellCol = cells[pointIndex].col;
                        matrix[cellRow, cellCol] = imgIndex;
                        cells.RemoveAt (pointIndex);
					} catch (Exception e) {
                        Log.Verbose ("i: " + i);
					}
				}
				i++;
			}
		} while (i < (row-2) * (col-2) / 2);
        return matrix;
    }


	public static List<Cell> GetWayBetweenTwoCell (int[,] matrix, Cell cell1, Cell cell2)
    {
        List<Cell> cellsOnLine = new List<Cell> ();
        if (matrix[cell1.row, cell1.col] == 0 || matrix[cell2.row, cell2.col]  == 0)
            return cellsOnLine;

		if (!(cell1.row == cell2.row && cell1.col == cell2.col) && matrix[cell1.row, cell1.col] == matrix[cell2.row, cell2.col]) {
			// check line with x
			if (cell1.row == cell2.row) {
				Log.Verbose ("line x");
				if (CheckLineX(matrix, cell1.col, cell2.col, cell1.row)) {
					Log.Verbose ("ok line x");
                    cellsOnLine.Add (cell1);
                    cellsOnLine.Add (cell2);
					return cellsOnLine;
				}
			}
			// check line with y
			if (cell1.col == cell2.col) {
				Log.Verbose ("line y");
				if (CheckLineY(matrix, cell1.row, cell2.row, cell1.col)) {
					Log.Verbose ("ok line y");
                    cellsOnLine.Add (cell1);
                    cellsOnLine.Add (cell2);
					return cellsOnLine;
				}
			}

			int t = -1; // t is column find

			// check in rectangle with x
			if ((t = CheckRectX(matrix, cell1, cell2)) != -1) {
				Log.Verbose ("rect x");
                cellsOnLine.Add (cell1);
                cellsOnLine.Add (new Cell (cell1.row, t));
                cellsOnLine.Add (new Cell (cell2.row, t));
                cellsOnLine.Add (cell2);
                return cellsOnLine;
				// return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
			}

			// check in rectangle with y
			if ((t = CheckRectY (matrix, cell1, cell2)) != -1) {
				Log.Verbose ("rect y");
                cellsOnLine.Add (cell1);
                cellsOnLine.Add (new Cell (t, cell1.col));
                cellsOnLine.Add (new Cell (t, cell2.col));
                cellsOnLine.Add (cell2);

                return cellsOnLine;
				// return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
			}
			// check more right
			if ((t = CheckMoreLineX (matrix, cell1, cell2, 1)) != -1) {
				Log.Verbose ("more right");
                cellsOnLine.Add (cell1);
                cellsOnLine.Add (new Cell (cell1.row, t));
                cellsOnLine.Add (new Cell (cell2.row, t));
                cellsOnLine.Add (cell2);

                return cellsOnLine;
				// return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
			}
			// check more left
			if ((t = CheckMoreLineX (matrix, cell1, cell2, -1)) != -1) {
				Log.Verbose ("more left");
                cellsOnLine.Add (cell1);
                cellsOnLine.Add (new Cell (cell1.row, t));
                cellsOnLine.Add (new Cell (cell2.row, t));
                cellsOnLine.Add (cell2);

                return cellsOnLine;
				// return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
			}
			// check more down
			if ((t = CheckMoreLineY (matrix, cell1, cell2, 1)) != -1) {
				Log.Verbose ("more down");
                cellsOnLine.Add (cell1);
                cellsOnLine.Add (new Cell (t, cell1.col));
                cellsOnLine.Add (new Cell (t, cell2.col));
                cellsOnLine.Add (cell2);

                return cellsOnLine;
				// return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
			}
			// check more up
			if ((t = CheckMoreLineY(matrix, cell1, cell2, -1)) != -1) {
				Log.Verbose ("more up");
                cellsOnLine.Add (cell1);
                cellsOnLine.Add (new Cell (t, cell1.col));
                cellsOnLine.Add (new Cell (t, cell2.col));
                cellsOnLine.Add (cell2);

                return cellsOnLine;
				// return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
			}
		}
		return cellsOnLine;
	}


	// check with line x, from column y1 to y2
	private static bool CheckLineX(int[,] matrix, int y1, int y2, int x) {
		Log.Verbose ("check line x");
		// find point have column max and min
		int min = Math.Min (y1, y2);
		int max = Math.Max (y1, y2);
		// run column
		for (int y = min + 1; y < max; y++) {
    		// Log.Verbose ("x: " + x + " y: " + y + "value: " + matrix[x,y]);        
			if (matrix[x, y] > Consts.NotBarrier) { // if see barrier then die
				Log.Verbose ("die: " + x + "" + y);
				return false;
			}
            else {
    			Log.Verbose ("ok: " + x + "" + y);            
            }
		}
		// not die -> success
		return true;
	}

	private static bool CheckLineY(int[,] matrix, int x1, int x2, int y) {
		Log.Verbose ("check line y");
		int min = Math.Min (x1, x2);
		int max = Math.Max (x1, x2);
		for (int x = min + 1; x < max; x++) {
			if (matrix[x, y] > Consts.NotBarrier) {
				Log.Verbose ("die: " + x + "" + y);
				return false;
			}
			Log.Verbose ("ok: " + x + "" + y);
		}
		return true;
	}

	// check in rectangle
	private static int CheckRectX(int[,] matrix, Cell cell1, Cell cell2) {
		Log.Verbose ("check rect x");
		// find point have y min and max
		Cell minColCell = cell1, maxColCell = cell2;
		if (cell1.col > cell2.col) {
			minColCell = cell2;
			maxColCell = cell1;
		}
		for (int y = minColCell.col; y <= maxColCell.col; y++) {
			if (y > minColCell.col && matrix[minColCell.row, y] > Consts.NotBarrier) {
				return -1;
			}
			// check two line
			if ((matrix[maxColCell.row, y] == Consts.NotBarrier || y == maxColCell.col)
					&& CheckLineY(matrix, minColCell.row, maxColCell.row, y)
					&& CheckLineX(matrix, y, maxColCell.col, maxColCell.row)) {

				Log.Verbose ("Rect x");
				Log.Verbose("(" + minColCell.row + "," + minColCell.col + ") -> ("
						+ minColCell.row + "," + y + ") -> (" + maxColCell.row + "," + y
						+ ") -> (" + maxColCell.row + "," + maxColCell.col + ")");
				// if three line is true return column y
				return y;
			}
		}
		// have a line in three line not true then return -1
		return -1;
	}

	private static int CheckRectY(int[,] matrix, Cell cell1, Cell cell2) {
		Log.Verbose ("check rect y");
		// find point have y min
		Cell minRowCell = cell1, maxRowCell = cell2;
		if (cell1.row > cell2.row) {
			minRowCell = cell2;
			maxRowCell = cell1;
		}
		// find line and y begin
		for (int x = minRowCell.row; x <= maxRowCell.row; x++) {
			if (x > minRowCell.row && matrix[x, minRowCell.col] > Consts.NotBarrier) {
				return -1;
			}
			if ((matrix[x, maxRowCell.col] == Consts.NotBarrier || x == maxRowCell.row)
					&& CheckLineX (matrix, minRowCell.col, maxRowCell.col, x)
					&& CheckLineY (matrix, x, maxRowCell.row, maxRowCell.col)) {

				Log.Verbose ("Rect y");
				Log.Verbose ("(" + minRowCell.row + "," + minRowCell.col + ") -> (" + x
						+ "," + minRowCell.col + ") -> (" + x + "," + maxRowCell.col
						+ ") -> (" + maxRowCell.row + "," + maxRowCell.col + ")");
				return x;
			}
		}
		return -1;
	}

	/**
	 * p1 and p2 are Points want check
	 * 
	 * @param type
	 *            : true is check with increase, false is decrease return column
	 *            can connect p1 and p2
	 */
	private static int CheckMoreLineX (int[,] matrix, Cell cell1, Cell cell2, int type) {
		Log.Verbose ("check chec more x");
		// find point have y min
		Cell minColCell = cell1, maxColCell = cell2;
		if (cell1.col > cell2.col) {
			minColCell = cell2;
			maxColCell = cell1;
		}
		// find line and y begin
		int y = maxColCell.col + type;
		int row = minColCell.row;
		int colFinish = maxColCell.col;
		if (type == -1) {
			colFinish = minColCell.col;
			y = minColCell.col + type;
			row = maxColCell.row;
			Log.Verbose ("colFinish = " + colFinish);
		}

		// find column finish of line

		// check more
		if ((matrix[row, colFinish] == Consts.NotBarrier || minColCell.col == maxColCell.col)
				&& CheckLineX (matrix, minColCell.col, maxColCell.col, row)) {
			while (matrix[minColCell.row, y] == Consts.NotBarrier
					&& matrix[maxColCell.row, y] == Consts.NotBarrier) {
				if (CheckLineY (matrix, minColCell.row, maxColCell.row, y)) {

					Log.Verbose ("TH X " + type);
					Log.Verbose ("(" + minColCell.row + "," + minColCell.row + ") -> ("
							+ minColCell.row + "," + y + ") -> (" + maxColCell.row + "," + y
							+ ") -> (" + maxColCell.row + "," + maxColCell.row + ")");
					return y;
				}
				y += type;
			}
		}
		return -1;
	}

	private static int CheckMoreLineY(int[,] matrix, Cell cell1, Cell cell2, int type) {
		Log.Verbose ("check more y");
		Cell minRowCell = cell1, maxRowCell = cell2;
		if (cell1.row > cell2.row) {
			minRowCell = cell2;
			maxRowCell = cell1;
		}
		int x = maxRowCell.row + type;
		int col = minRowCell.col;
		int rowFinish = maxRowCell.row;
		if (type == -1) {
			rowFinish = minRowCell.row;
			x = minRowCell.row + type;
			col = maxRowCell.col;
		}
		if ((matrix[rowFinish, col] == Consts.NotBarrier || minRowCell.row == maxRowCell.row)
				&& CheckLineY (matrix, minRowCell.row, maxRowCell.row, col)) {
			while (matrix[x, minRowCell.col] == Consts.NotBarrier
					&& matrix[x, maxRowCell.col] == Consts.NotBarrier) {
				if (CheckLineX (matrix, minRowCell.col, maxRowCell.col, x)) {
					Log.Verbose ("TH Y " + type);
					Log.Verbose ("(" + minRowCell.row + "," + minRowCell.col + ") -> ("
							+ x + "," + minRowCell.col + ") -> (" + x + "," + maxRowCell.col
							+ ") -> (" + maxRowCell.row + "," + maxRowCell.col + ")");
					return x;
				}
				x += type;
			}
		}
		return -1;
	}


    
}