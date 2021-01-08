using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace MorCohen
{
    public class ResourceHandler: IResourceHandler
    {
        private Action<Socket> m_handler;
        private Socket m_socket;

        /// <summary>
        /// Represents a resource handler that operates synchronously.
        /// </summary>
        public ResourceHandler(Action<Socket> handler, Socket socket)
        {
            m_handler = handler;
            m_socket = socket;
        }

        public void Call()
        {
            m_handler?.Invoke(m_socket);
        }
    }
}
