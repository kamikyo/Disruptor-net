namespace Disruptor.Tests.Support
{
    public class DummySequenceBarrierAlert : SequenceBarrierAlert
    {
        public DummySequenceBarrierAlert(ISequenceBarrier sequenceBarrier = null)
            : base(sequenceBarrier ?? new DummySequenceBarrier())
        {
        }
    }
}
