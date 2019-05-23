public class Cell {
    public int row = 0;
    public int col = 0;
    public Cell (int row, int col)
    {
        this.row = row;
        this.col = col;
    }
    public override string ToString ()
    {
        return "(" + row + "," + col + ")";   
    }
}
