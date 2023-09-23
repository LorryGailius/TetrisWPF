using TetrisWPF.Blocks;

namespace TetrisWPF
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            set
            {
                currentBlock = value;
                currentBlock.Reset();
            }
        }

        public GameGrid Grid { get; }
        public BlockQueue BlockQueue { get; }

        public bool GameOver { get; set; }

        public GameState(int rows, int columns)
        {
            Grid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetNextBlock();
        }

        private bool BlockFits() 
        { 
            foreach (Position p in CurrentBlock.GetTiles())
            {
                if (!Grid.IsCellEmpty(p.Row, p.Column))
                {
                    return false; 
                } 
            }
            return true;
        }

        public void RotateBlockClockwise()
        {
            CurrentBlock.RotateClockwise();
            if (!BlockFits())
            {
                CurrentBlock.RotateCounterClockwise();
            }
        }

        public void RotateBlockCounterClockwise()
        {
            CurrentBlock.RotateCounterClockwise();
            if (!BlockFits())
            {
                CurrentBlock.RotateClockwise();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        private bool IsGameOver()
        {
            return !(Grid.IsRowEmpty(0) && Grid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.GetTiles())
            {
                Grid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Grid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetNextBlock();
            } 
        }
    }
}
