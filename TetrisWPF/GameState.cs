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

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid Grid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; set; }
        public int Score { get; set; }
        public Block heldBlock { get; set; }
        public bool CanHold { get; set; } = true;

        public GameState(int rows, int columns)
        {
            Grid = new GameGrid(rows, columns);
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

        public void HoldBlock()
        {
            if (CanHold)
            {
                if (heldBlock == null)
                {
                    heldBlock = CurrentBlock;
                    CurrentBlock = BlockQueue.GetNextBlock();
                }
                else
                {
                    Block temp = heldBlock;
                    heldBlock = CurrentBlock;
                    CurrentBlock = temp;
                }
                CanHold = false;
            }
        }

        public void RotateBlockClockwise()
        {
            CurrentBlock.RotateClockwise();
            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);

                if (!BlockFits())
                {
                    CurrentBlock.Move(0, 2);
                    CurrentBlock.RotateClockwise();
                }
            }
        }

        public void RotateBlockCounterClockwise()
        {
            CurrentBlock.RotateCounterClockwise();
            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);

                if (!BlockFits())
                {
                    CurrentBlock.Move(0, 2);
                    CurrentBlock.RotateCounterClockwise();
                }
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

            Score += Grid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetNextBlock();
                CanHold = true;
            }
        }
    }
}
