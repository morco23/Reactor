using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MorCohen
{
    class Demultiplexer
    {
        private Dispatcher m_dispatcher;
        private bool m_shouldRun;

        /// <summary>
        /// Implements the Synchronous Event Demultiplexer of the Reactor. This part monitor the socket
        /// and in case of event send them to the dispatcher.
        /// </summary>
        public Demultiplexer(Dispatcher dispatcher)
        {
            m_dispatcher = dispatcher;
        }

        public void Start()
        {
            m_shouldRun = true;
            List<Socket> readSockets, writeSockets, errorSockets;
            while (m_shouldRun)
            {
                readSockets = m_dispatcher.GetSockets(ResourceActionType.READ);
                writeSockets = m_dispatcher.GetSockets(ResourceActionType.WRITE);
                errorSockets = m_dispatcher.GetSockets(ResourceActionType.ERROR);

                Socket.Select(readSockets, writeSockets, errorSockets, -1);

                m_dispatcher.Dispatch(readSockets, ResourceActionType.READ);
                m_dispatcher.Dispatch(writeSockets, ResourceActionType.WRITE);
                m_dispatcher.Dispatch(errorSockets, ResourceActionType.ERROR);
            }

        }

        public void Stop()
        {
            m_shouldRun = false;
        }
            
    
    }
}
