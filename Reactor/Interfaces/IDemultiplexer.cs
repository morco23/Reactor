using System;
using System.Collections.Generic;
using System.Text;

namespace MorCohen.Interfaces
{
    /// <summary>
    /// The Synchronous Event Demultiplexer of the Reactor. This part monitors the sockets
    /// and in case of event send them to the dispatcher.
    /// </summary>
    public interface IDemultiplexer: IDisposable
    {
        /// <summary>
        /// Start the demultiplexer. Scan the sockets and when there is an event, call to action.
        /// </summary>
        public void Start();

        /// <summary>
        /// Stop the demultiplexer.
        /// </summary>
        public void Stop();
    }
}
