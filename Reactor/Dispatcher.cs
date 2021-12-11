using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using MorCohen.Interfaces;

namespace MorCohen
{
    class Dispatcher: IDispatcher
    {
        private Dictionary<ResourceActionType, Dictionary<Socket, Action<Socket>>> _resourceToActionDict;

        public Dispatcher()
        {
            _resourceToActionDict = new Dictionary<ResourceActionType, Dictionary<Socket, Action<Socket>>>();
            _resourceToActionDict[ResourceActionType.READ] = new Dictionary<Socket, Action<Socket>>();
            _resourceToActionDict[ResourceActionType.WRITE] = new Dictionary<Socket, Action<Socket>>();
            _resourceToActionDict[ResourceActionType.ERROR] = new Dictionary<Socket, Action<Socket>>();
        }

        public void Register(Socket socket, ResourceActionType actionType, Action<Socket> action)
        {
            _resourceToActionDict[actionType][socket] = action;
        }

        public void Unregister(Socket socket, ResourceActionType actionType)
        {
            _resourceToActionDict[actionType].Remove(socket);
        }

        public void Dispatch(List<Socket> sockets, ResourceActionType actionType)
        {
            sockets.ForEach(socket => _resourceToActionDict[actionType][socket](socket));
        }

        public List<Socket> GetSockets(ResourceActionType actionType)
        {
            return _resourceToActionDict[actionType].Keys.ToList();
        }
    }
}
