using System;
using System.Runtime.CompilerServices;

namespace Disruptor
{
    public readonly struct WaitResult : IEquatable<WaitResult>
    {
        private WaitResult(long nextAvailableSequence)
        {
            NextAvailableSequence = nextAvailableSequence;
        }

        public readonly long NextAvailableSequence;
        public bool IsValid => NextAvailableSequence >= 0;

        public bool TryGetSequence(out long sequence)
        {
            sequence = NextAvailableSequence;
            return NextAvailableSequence >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitResult Success(long nextAvailableSequence) => new WaitResult(nextAvailableSequence);

        public static readonly WaitResult Timeout = new WaitResult(-1);

        public static readonly WaitResult Cancel = new WaitResult(-2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(WaitResult left, WaitResult right)
        {
            return left.NextAvailableSequence == right.NextAvailableSequence;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(WaitResult left, WaitResult right)
        {
            return left.NextAvailableSequence != right.NextAvailableSequence;
        }

        public bool Equals(WaitResult other)
        {
            return NextAvailableSequence == other.NextAvailableSequence;
        }

        public override bool Equals(object obj)
        {
            return obj is WaitResult other && Equals(other);
        }

        public override int GetHashCode()
        {
            return NextAvailableSequence.GetHashCode();
        }
    }
}
