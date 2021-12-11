using MorCohen.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MorCohen
{
    class Demultiplexer:  IDemultiplexer
    {
        private IDispatcher _dispatcher;
        private bool m_shouldRun;
        private readonly Socket _dummySocket; // Used for select interruption.

        public Demultiplexer(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _dummySocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _dispatcher.Register(_dummySocket, ResourceActionType.ERROR, (socket) => { });
        }

        public void Start()
        {
            m_shouldRun = true;
            List<Socket> readSockets, writeSockets, errorSockets;
            while (m_shouldRun)
            {
                readSockets = _dispatcher.GetSockets(ResourceActionType.READ);
                writeSockets = _dispatcher.GetSockets(ResourceActionType.WRITE);
                errorSockets = _dispatcher.GetSockets(ResourceActionType.ERROR);

                Socket.Select(readSockets, writeSockets, errorSockets, -1);

                _dispatcher.Dispatch(readSockets, ResourceActionType.READ);
                _dispatcher.Dispatch(writeSockets, ResourceActionType.WRITE);
                _dispatcher.Dispatch(errorSockets, ResourceActionType.ERROR);
            }

        }

        public void Stop()
        {
            m_shouldRun = false;
            _dummySocket.Close();
        }

        public void Dispose()
        {
            _dummySocket.Dispose();
        }
    }
}
