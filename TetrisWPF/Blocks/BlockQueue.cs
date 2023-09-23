﻿using System;

namespace TetrisWPF.Blocks
{
    public class BlockQueue
    {
        private readonly Block[] blocks =
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };


        private readonly Random random = new Random();

        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetNextBlock()
        {
            var block = NextBlock;
            NextBlock = RandomBlock();
            return block;
        }
    }
}
