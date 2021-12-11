using MorCohen;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MorCohen.Interfaces
{
    public interface IDispatcher
    {
        /// <summary>
        /// Register a resource to the dispatcher.
        /// </summary>
        /// <param name="socket"> Socket to register </param>
        /// <param name="actionType"> The type of action. </param>
        /// <param name="action"> Action to call when there is an event. </param>
        void Register(Socket socket, ResourceActionType actionType, Action<Socket> action);

        /// <summary>
        /// Unregister a resource from the dispatcher.
        /// </summary>     
        /// <param name="socket"> Socket to unregister </param>
        /// <param name="actionType"> The type of action. </param>
        public void Unregister(Socket socket, ResourceActionType actionType);

        /// <summary>
        /// Call the action attached to each socket and actionType.
        /// </summary>
        /// <param name="actionType"> The type of action. </param>
        public void Dispatch(List<Socket> sockets, ResourceActionType actionType);

        /// <summary>
        /// Get a list of sockets for an action type.
        /// </summary>
        /// <param name="actionType"> The type of action. </param>
        public List<Socket> GetSockets(ResourceActionType actionType);
    }
}
