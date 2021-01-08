using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MorCohen
{
    public class Reactor
    {
        private Dispatcher m_dispatcher;
        private Demultiplexer m_demultiplexer;

        /// <summary>
        /// Implementation of Reactor design pattern.
        /// </summary>
        public Reactor()
        {
            m_dispatcher = new Dispatcher();
            m_demultiplexer = new Demultiplexer(m_dispatcher);
        }

        /// <summary>
        /// Start Reactor.
        /// </summary>
        public void Start()
        {
            m_demultiplexer.Start();
        }

        /// <summary>
        /// Stop Reactor.
        /// </summary>
        public void Stop()
        {
            m_demultiplexer.Stop();
        }

        /// <summary>
        /// Register a socket to reactor for some action.
        /// </summary>
        public void Register(Socket socket, ResourceActionType action,
                                              IResourceHandler handler)
        {
            m_dispatcher.Register(socket, action, handler);
        }

        /// <summary>
        /// Unregister a socket to reactor for some action.
        /// </summary>
        public void Unregister(Socket socket, ResourceActionType action)
        {
            m_dispatcher.Unregister(socket, action);
        }
    }
}
