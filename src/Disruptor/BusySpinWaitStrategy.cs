namespace Disruptor
{
    /// <summary>
    /// Busy Spin strategy that uses a busy spin loop for <see cref="IEventProcessor"/>s waiting on a barrier.
    /// 
    /// This strategy will use CPU resource to avoid syscalls which can introduce latency jitter.  It is best
    /// used when threads can be bound to specific CPU cores.
    /// </summary>
    public sealed class BusySpinWaitStrategy : INonBlockingWaitStrategy
    {
        /// <summary>
        /// <see cref="IWaitStrategy.WaitFor"/>
        /// </summary>
        public WaitResult WaitFor(long sequence, Sequence cursor, ISequence dependentSequence, SequenceBarrierAlert alert)
        {
            long availableSequence;

            while ((availableSequence = dependentSequence.Value) < sequence)
            {
                if (alert.IsActive)
                    return WaitResult.Cancel;
            }

            return WaitResult.Success(availableSequence);
        }

        /// <summary>
        /// <see cref="IWaitStrategy.SignalAllWhenBlocking"/>
        /// </summary>
        public void SignalAllWhenBlocking()
        {
        }
    }
}
