using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MorCohen
{
    class ResourceDictionary
    {
        private Dictionary<Socket, IResourceHandler>[] m_resources;

        /// <summary>
        /// Represent a resource dictionary of action/socket/handler.
        /// </summary>
        public ResourceDictionary()
        {
            m_resources = new Dictionary<Socket, IResourceHandler>[(int)ResourceActionType.LEN];
            for (int type = 0; type < (int)ResourceActionType.LEN; ++type)
            {
                m_resources[type] = new Dictionary<Socket, IResourceHandler>();
            }
        }

        public void Add(Socket socket, ResourceActionType action, IResourceHandler handler)
        {
            m_resources[(int)action][socket] = handler;
        }

        public void Remove(Socket socket, ResourceActionType action)
        {
            m_resources[(int)action].Remove(socket, out _);
        }

        public List<Socket> GetList(ResourceActionType action)
        {
            return new List<Socket>(m_resources[(int)action].Keys);
        }

        public IResourceHandler GetHandler(Socket socket, ResourceActionType action)
        {
            return m_resources[(int)action][socket];
        }


    }
}
