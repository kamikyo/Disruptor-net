namespace Disruptor.Tests.Support
{
    public class DummyNonBlockingWaitStrategy : INonBlockingWaitStrategy
    {
        public int SignalAllWhenBlockingCalls { get; private set; }

        public WaitResult WaitFor(long sequence, Sequence cursor, ISequence dependentSequence, SequenceBarrierAlert alert)
        {
            return WaitResult.Success(default);
        }

        public void SignalAllWhenBlocking()
        {
            SignalAllWhenBlockingCalls++;
        }
    }
}
