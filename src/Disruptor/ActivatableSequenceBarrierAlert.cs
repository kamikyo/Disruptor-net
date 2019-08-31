using System.Runtime.CompilerServices;

namespace Disruptor
{
    /// <summary>
    /// Represents the alert status for a <see cref="ISequenceBarrier"/>.
    /// </summary>
    public class ActivatableSequenceBarrierAlert : SequenceBarrierAlert
    {
        public ActivatableSequenceBarrierAlert(ISequenceBarrier sequenceBarrier) : base(sequenceBarrier)
        {
        }

        /// <summary>
        /// Activate the alert.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivate the current alert status.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
