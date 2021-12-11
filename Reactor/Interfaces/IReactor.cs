using MorCohen;
using System;
using System.Net.Sockets;

namespace MorCohen.Interfaces
{
    public interface IReactor: IDisposable
    {
        /// <summary>
        /// Start the reactor.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the reactor.
        /// </summary>
        void Stop();

        /// <summary>
        /// Register a socket to the reactor.
        /// </summary>
        /// <param name="socket"> Socket to register </param>
        /// <param name="actionType"> The type of action. </param>
        /// <param name="action"> Action to call when there is an event. </param>
        public void Register(Socket socket, ResourceActionType actionType, Action<Socket> action);

        /// <summary>
        /// Unregister a socket from the reactor.
        /// </summary>
        /// <param name="socket"> Socket to unregister </param>
        /// <param name="actionType"> The type of action. </param>
        public void Unregister(Socket socket, ResourceActionType actionType);
    }
}
