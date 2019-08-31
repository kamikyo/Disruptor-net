using System.Runtime.CompilerServices;

namespace Disruptor
{
    /// <summary>
    /// Represents the read-only alert status for a <see cref="ISequenceBarrier"/>.
    /// </summary>
    public abstract class SequenceBarrierAlert
    {
        private readonly ISequenceBarrier _sequenceBarrier;
        private volatile bool _isActive;

        protected SequenceBarrierAlert(ISequenceBarrier sequenceBarrier)
        {
            _sequenceBarrier = sequenceBarrier;
        }

        /// <summary>
        /// Gets the current alert status for the barrier.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            protected set => _isActive = value;
        }

        /// <summary>
        /// Check if an alert has been raised and throw an <see cref="AlertException"/> if it has.
        /// </summary>
        /// <exception cref="AlertException">if alert has been raised.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Check()
        {
            if (_isActive)
            {
                AlertException.Throw();
            }
        }

        /// <summary>
        /// Indicates whether the alert is associated to the specified <see cref="ISequenceBarrier"/>.
        /// Can be used by a <see cref="IWaitStrategy"/> to identify the current sequence barrier.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAssociatedTo(ISequenceBarrier sequenceBarrier)
        {
            return _sequenceBarrier == sequenceBarrier;
        }
    }
}
