using System.Threading.Tasks;

namespace Disruptor.Dsl
{
    public interface IConsumerInfo
    {
        ISequence[] Sequences { get; }

        ISequenceBarrier Barrier { get; }

        bool IsEndOfChain { get; }

        void Start(TaskScheduler taskScheduler);

        Task StartAsync(TaskScheduler taskScheduler);

        void Halt();

        void MarkAsUsedInBarrier();

        bool IsRunning { get; }
    }
}
