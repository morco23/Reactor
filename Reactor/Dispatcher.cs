using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace MorCohen
{
    class Dispatcher
    {
        private ResourceDictionary m_resourceDictionary;

        /// <summary>
        /// Implements of the dispatcher part of Reactor design pattern.
        /// </summary>
        /// </param>
        public Dispatcher()
        {
            m_resourceDictionary = new ResourceDictionary();
        }

        /// <summary>
        /// Register a resource to the dispatcher.
        /// </summary>
        /// <param name="socket">Socket to register</param>
        /// <param name="action">The type of action.</param>
        /// <param name="handler">Handler to call on event</param>
        public void Register(Socket socket, ResourceActionType action,
                                              IResourceHandler handler)
        {
            m_resourceDictionary.Add(socket, action, handler);
        }

        /// <summary>
        /// Unregister a resource from the dispatcher.
        /// </summary>
        public void Unregister(Socket socket, ResourceActionType action)
        {
            m_resourceDictionary.Remove(socket, action);
        }

        /// <summary>
        /// Call the handler of each socket in the action list.
        /// </summary>
        public void Dispatch(List<Socket> sockets, ResourceActionType action)
        {
            sockets.ForEach((socket) => m_resourceDictionary.GetHandler(socket, action)?.Call());
        }

        /// <summary>
        /// Get list of the sockets for action type.
        /// </summary>
        public List<Socket> GetSockets(ResourceActionType action)
        {
            return m_resourceDictionary.GetList(action);
        }
    }


}
