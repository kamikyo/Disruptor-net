using System.Runtime.CompilerServices;

namespace Disruptor
{
    public readonly struct WaitResult
    {
        private WaitResult(long nextAvailableSequence, WaitResultType type)
        {
            NextAvailableSequence = nextAvailableSequence;
            Type = type;
        }

        public readonly long NextAvailableSequence;
        public readonly WaitResultType Type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitResult Success(long nextAvailableSequence)
            => new WaitResult(nextAvailableSequence, WaitResultType.Success);

        public static readonly WaitResult Timeout = new WaitResult(default, WaitResultType.Timeout);

        public static readonly WaitResult Cancel = new WaitResult(default, WaitResultType.Cancel);
    }
}
