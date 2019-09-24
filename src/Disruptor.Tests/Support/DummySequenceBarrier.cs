namespace Disruptor.Tests.Support
{
    public class DummySequenceBarrier : ISequenceBarrier
    {
        public WaitResult WaitFor(long sequence)
        {
            return WaitResult.Success(default);
        }

        public long Cursor => 0;
        public bool IsAlerted => false;

        public void Alert()
        {
        }

        public void ClearAlert()
        {
        }
    }
}
