using System;
using System.Threading;

namespace Disruptor
{
    /// <summary>
    /// Variation of the <see cref="TimeoutBlockingWaitStrategy"/> that attempts to elide conditional wake-ups
    /// when the lock is uncontended.
    /// </summary>
    public class LiteTimeoutBlockingWaitStrategy : IWaitStrategy
    {
        private readonly object _lock = new object();
        private volatile int _signalNeeded;
        private readonly int _timeoutInMilliseconds;

        /// <summary>
        /// Creates a <see cref="LiteTimeoutBlockingWaitStrategy"/> with the specified timeout.
        /// </summary>
        public LiteTimeoutBlockingWaitStrategy(TimeSpan timeout)
        {
            _timeoutInMilliseconds = (int)timeout.TotalMilliseconds;
        }

        /// <summary>
        /// <see cref="IWaitStrategy.WaitFor"/>.
        /// </summary>
        public WaitResult WaitFor(long sequence, Sequence cursor, ISequence dependentSequence, SequenceBarrierAlert alert)
        {
            var milliseconds = _timeoutInMilliseconds;

            long availableSequence;
            if (cursor.Value < sequence)
            {
                lock (_lock)
                {
                    while (cursor.Value < sequence)
                    {
                        Interlocked.Exchange(ref _signalNeeded, 1);

                        if (alert.IsActive)
                            return WaitResult.Cancel;

                        if (!Monitor.Wait(_lock, milliseconds))
                            return WaitResult.Timeout;
                    }
                }
            }

            var aggressiveSpinWait = new AggressiveSpinWait();
            while ((availableSequence = dependentSequence.Value) < sequence)
            {
                if (alert.IsActive)
                    return WaitResult.Cancel;

                aggressiveSpinWait.SpinOnce();
            }

            return WaitResult.Success(availableSequence);
        }

        /// <summary>
        /// <see cref="IWaitStrategy.SignalAllWhenBlocking"/>.
        /// </summary>
        public void SignalAllWhenBlocking()
        {
            if (Interlocked.Exchange(ref _signalNeeded, 0) == 1)
            {
                lock (_lock)
                {
                    Monitor.PulseAll(_lock);
                }
            }
        }
    }
}
