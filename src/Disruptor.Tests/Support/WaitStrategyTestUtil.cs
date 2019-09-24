using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Disruptor.Tests.Support
{
    public static class WaitStrategyTestUtil
    {
        public static void AssertWaitForWithDelayOf(long sleepTimeMillis, IWaitStrategy waitStrategy)
        {
            var sequenceUpdater = new SequenceUpdater(sleepTimeMillis, waitStrategy);
            Task.Factory.StartNew(sequenceUpdater.Run);
            sequenceUpdater.WaitForStartup();
            var cursor = new Sequence(0);
            var alert = new DummySequenceBarrierAlert();
            var sequence = waitStrategy.WaitFor(0, cursor, sequenceUpdater.Sequence, alert).GetResultOrThrow();

            Assert.That(sequence, Is.EqualTo(0L));
        }

        public static long GetWaitResultOrThrow(this ISequenceBarrier sequenceBarrier, long sequence)
        {
            var waitResult = sequenceBarrier.WaitFor(sequence);
            return waitResult.GetResultOrThrow();
        }

        public static long GetResultOrThrow(this WaitResult waitResult)
        {
            if (waitResult.Type != WaitResultType.Success)
                throw new Exception();

            return waitResult.NextAvailableSequence;
        }
    }
}
