namespace TetrisWPF
{
    public class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int row, int column]
        {
            get => grid[row, column];
            set => grid[row, column] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInsideGrid(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        public bool IsCellEmpty(int row, int column)
        {
            return IsInsideGrid(row, column) && grid[row, column] == 0;
        }

        public bool IsRowFull(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowEmpty(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void ClearRow(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row, column] = 0;
            }
        }

        private void MoveRowDown(int row, int numRows)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row + numRows, column] = grid[row, column];
                grid[row, column] = 0;
            }
        }

        public int ClearFullRows()
        {
            int numRowsMoved = 0;
            for (int row = 0; row < Rows; row++)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    numRowsMoved++;
                }
                else if (numRowsMoved > 0)
                {
                    MoveRowDown(row, numRowsMoved);
                }
            }
            return numRowsMoved;
        }
    }
}
