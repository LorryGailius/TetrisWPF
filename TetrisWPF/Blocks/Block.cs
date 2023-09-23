using System.Collections.Generic;

namespace TetrisWPF.Blocks
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }

        private int rotationIndex;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Position> GetTiles()
        {
            foreach (Position p in Tiles[rotationIndex])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        public void RotateClockwise()
        {
            rotationIndex = (rotationIndex + 1) % Tiles.Length;
        }

        public void RotateCounterClockwise()
        {
            if (rotationIndex == 0)
            {
                rotationIndex = Tiles.Length - 1;
            }
            else
            {
                rotationIndex--;
            }
        }

        public void Move(int rows, int columns)
        {
            offset = new Position(offset.Row + rows, offset.Column + columns);
        }

        public void Reset()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
            rotationIndex = 0;
        }
    }
}
