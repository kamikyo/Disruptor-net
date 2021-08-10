using System.Threading;
using System.Threading.Tasks;

namespace Disruptor
{
    public static class EventProcessorExtensions
    {
        public static Task Start(this IEventProcessor eventProcessor)
        {
            return eventProcessor.Start(TaskScheduler.Default);
        }

        public static Task Start(this IEventProcessor eventProcessor, TaskScheduler taskScheduler)
        {
            return Task.Factory.StartNew(eventProcessor.Run, CancellationToken.None, TaskCreationOptions.LongRunning, taskScheduler);
        }

        public static async Task StartAsync(this IEventProcessor eventProcessor, TaskScheduler taskScheduler)
        {
            await Task.Factory.StartNew(eventProcessor.RunAsync, CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }
    }
}
