﻿using NBitcoin;
using Stratis.Bitcoin.Utilities;

namespace Stratis.Bitcoin.BlockStore
{
    public sealed class BlockPair
    {
        /// <summary>
        /// Construct BlockPair with a Block and ChainedBlock instance to ensure that its valid
        /// </summary>
        public BlockPair(Block block, ChainedBlock chainedBlock)
        {
            Guard.NotNull(block, "block");
            Guard.NotNull(chainedBlock, "chainedBlock");

            this.Block = block;
            this.ChainedBlock = chainedBlock;
        }

        public Block Block { get; private set; }
        public ChainedBlock ChainedBlock { get; private set; }
    }
}