using MorCohen.Interfaces;
using System;
using System.Net.Sockets;

namespace MorCohen
{
    public class Reactor: IReactor
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDemultiplexer _demultiplexer;

        public Reactor(IDemultiplexer demultiplexer, IDispatcher dispatcher)
        {
            _demultiplexer = demultiplexer;
            _dispatcher = dispatcher;
        }

        public void Start()
        {
            _demultiplexer.Start();
        }

        public void Stop()
        {
            _demultiplexer.Stop();
        }

        public void Register(Socket socket, ResourceActionType actionType, Action<Socket> action)
        {
            _dispatcher.Register(socket, actionType, action);
        }

        public void Unregister(Socket socket, ResourceActionType actionType)
        {
            _dispatcher.Unregister(socket, actionType);
        }

        public void Dispose()
        {
            _demultiplexer.Dispose();
        }
    }
}
