﻿using System.Threading.Tasks;

namespace Disruptor
{
    /// <summary>
    /// Callback interface to be implemented for processing events as they become available in the <see cref="RingBuffer{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of events for sharing during exchange or parallel coordination of an event.</typeparam>
    /// <remarks>See <see cref="IBatchEventProcessor{T}.SetExceptionHandler"/> if you want to handle exceptions propagated out of the handler.</remarks>
    public interface IEventHandler<in T>
    {
        /// <summary>
        /// Called when a publisher has committed an event to the <see cref="RingBuffer{T}"/>. The <see cref="IBatchEventProcessor{T}"/> will
        /// read messages from the <see cref="RingBuffer{T}"/> in batches, where a batch is all of the events available to be
        /// processed without having to wait for any new event to arrive.  This can be useful for event handlers that need
        /// to do slower operations like I/O as they can group together the data from multiple events into a single
        /// operation.  Implementations should ensure that the operation is always performed when endOfBatch is true as
        /// the time between that message an the next one is indeterminate.
        /// </summary>
        /// <param name="data">Data committed to the <see cref="RingBuffer{T}"/></param>
        /// <param name="sequence">Sequence number committed to the <see cref="RingBuffer{T}"/></param>
        /// <param name="endOfBatch">flag to indicate if this is the last event in a batch from the <see cref="RingBuffer{T}"/></param>
        void OnEvent(T data, long sequence, bool endOfBatch);

        Task OnEventAsync(T data, long sequence, bool endOfBatch);
    }
}
