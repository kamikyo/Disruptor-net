using System;
using System.Threading;

namespace Disruptor
{
    public class TimeoutBlockingWaitStrategy : IWaitStrategy
    {
        private readonly object _gate = new object();
        private readonly TimeSpan _timeout;

        public TimeoutBlockingWaitStrategy(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        /// <summary>
        /// <see cref="IWaitStrategy.WaitFor"/>
        /// </summary>
        public WaitResult WaitFor(long sequence, Sequence cursor, ISequence dependentSequence, SequenceBarrierAlert alert)
        {
            var timeSpan = _timeout;
            if (cursor.Value < sequence)
            {
                lock (_gate)
                {
                    while (cursor.Value < sequence)
                    {
                        if (alert.IsActive)
                            return WaitResult.Cancel;

                        if (!Monitor.Wait(_gate, timeSpan))
                        {
                            return WaitResult.Timeout;
                        }
                    }
                }
            }

            var aggressiveSpinWait = new AggressiveSpinWait();
            long availableSequence;
            while ((availableSequence = dependentSequence.Value) < sequence)
            {
                if (alert.IsActive)
                    return WaitResult.Cancel;

                aggressiveSpinWait.SpinOnce();
            }

            return WaitResult.Success(availableSequence);
        }

        /// <summary>
        /// <see cref="IWaitStrategy.SignalAllWhenBlocking"/>
        /// </summary>
        public void SignalAllWhenBlocking()
        {
            lock (_gate)
            {
                Monitor.PulseAll(_gate);
            }
        }
    }
}
