using System.Threading;

namespace Disruptor
{
    /// <summary>
    /// Yielding strategy that uses a Thread.Yield() for <see cref="IEventProcessor"/>s waiting on a barrier
    /// after an initially spinning.
    /// 
    /// This strategy is a good compromise between performance and CPU resource without incurring significant latency spikes.
    /// </summary>
    public sealed class YieldingWaitStrategy : INonBlockingWaitStrategy
    {
        private const int _spinTries = 100;

        /// <summary>
        /// <see cref="IWaitStrategy.WaitFor"/>
        /// </summary>
        public long WaitFor(long sequence, Sequence cursor, ISequence dependentSequence, SequenceBarrierAlert alert)
        {
            long availableSequence;
            var counter = _spinTries;

            while ((availableSequence = dependentSequence.Value) < sequence)
            {
                counter = ApplyWaitMethod(alert, counter);
            }

            return availableSequence;
        }

        /// <summary>
        /// Signal those <see cref="IEventProcessor"/> waiting that the cursor has advanced.
        /// </summary>
        public void SignalAllWhenBlocking()
        {
        }

        private static int ApplyWaitMethod(SequenceBarrierAlert alert, int counter)
        {
            alert.Check();

            if (counter == 0)
            {
                Thread.Yield();
            }
            else
            {
                --counter;
            }

            return counter;
        }
    }
}
