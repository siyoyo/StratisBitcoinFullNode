﻿using NBitcoin;
using Stratis.Bitcoin.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stratis.Bitcoin.Features.BlockStore.LoopSteps
{
    internal sealed class BlockStoreStepChain
    {
        private List<BlockStoreLoopStep> steps = new List<BlockStoreLoopStep>();

        internal void SetNextStep(BlockStoreLoopStep step)
        {
            this.steps.Add(step);
        }

        internal async Task<BlockStoreLoopStepResult> Execute(ChainedBlock nextChainedBlock, bool disposeMode, CancellationToken cancellationToken)
        {
            var result = BlockStoreLoopStepResult.Next();

            foreach (var step in this.steps)
            {
                var stepResult = await step.Execute(nextChainedBlock, cancellationToken, disposeMode);

                if (stepResult.ShouldBreak || stepResult.ShouldContinue)
                {
                    result = stepResult;
                    break;
                }
            }

            return result;
        }
    }

    internal abstract class BlockStoreLoopStep
    {
        protected BlockStoreLoopStep(BlockStoreLoop blockStoreLoop)
        {
            Guard.NotNull(blockStoreLoop, "blockStoreLoop");

            this.BlockStoreLoop = blockStoreLoop;
        }

        internal BlockStoreLoop BlockStoreLoop;

        internal abstract Task<BlockStoreLoopStepResult> Execute(ChainedBlock nextChainedBlock, CancellationToken cancellationToken, bool disposeMode);
    }

    internal sealed class BlockStoreLoopStepResult
    {
        internal BlockStoreLoopStepResult() { }

        internal bool ShouldBreak { get; private set; }
        internal bool ShouldContinue { get; private set; }

        internal static BlockStoreLoopStepResult Break()
        {
            var result = new BlockStoreLoopStepResult();
            result.ShouldBreak = true;
            return result;
        }

        internal static BlockStoreLoopStepResult Continue()
        {
            var result = new BlockStoreLoopStepResult();
            result.ShouldContinue = true;
            return result;
        }

        internal static BlockStoreLoopStepResult Next()
        {
            var result = new BlockStoreLoopStepResult();
            return result;
        }
    }
}