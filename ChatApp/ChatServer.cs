using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MorCohen;

namespace ChatApp
{
    public class ChatServer
    {
        private readonly TcpListener _listener;
        private readonly Reactor _reactor;
        private readonly List<Socket> _clients;
        private readonly int _port;

        /// <summary>
        /// Represents a chat server. Waiting for new connection, get messages and broadcast the message to all connections.
        /// </summary>
        /// <param name="port"> The port of the server.</param>
        public ChatServer(int port)
        {
            _listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            _reactor = ReactorBuilder.CreateReactor();
            _clients = new List<Socket>();
            _port = port;
        }

        #region Private
        private void WaitConnectionsRequests(Socket socket)
        {
            Socket clientSocket = _listener.AcceptSocket();
            _clients.Add(clientSocket);
            _reactor.Register(clientSocket, ResourceActionType.READ, GetMessageAndBroadcast);
        }

        private void GetMessageAndBroadcast(Socket socket)
        {
            try
            {
                string message = new StreamReader(new NetworkStream(socket)).ReadLine();
                if (message == null)
                {
                    CloseConnection(socket);
                    return;
                }
                BroadcastMassage(message, socket);
            }
            catch (Exception ex)
            {
                IPEndPoint endpoint = (IPEndPoint)socket.RemoteEndPoint;
                Console.WriteLine($"An error occurred. Connection of {endpoint.Address}:{endpoint.Port} will be closed.\nEx: {ex}.");
                CloseConnection(socket);
            }
        }

        private void BroadcastMassage(string message, Socket socket)
        {
            _clients.Where(_clients => _clients != socket).ToList()
                    .ForEach((socketClient) =>
                    {
                            _reactor.Register(socketClient, ResourceActionType.WRITE,
                                (socketClient) => SendMessage(message, socketClient));
                    });
        }

        private void SendMessage(string message, Socket socket)
        {
            try
            {
                StreamWriter sw = new StreamWriter(new NetworkStream(socket));
                sw.WriteLine(message);
                sw.Flush();
            }
            catch (Exception ex)
            {
                IPEndPoint endpoint = (IPEndPoint)socket.RemoteEndPoint;
                Console.WriteLine($"Unable to send message to client on address {endpoint.Address} : {endpoint.Port}. Connection will be closed. Ex: {ex}." );
                _clients.Remove(socket);
                socket.Close();
            }
            _reactor.Unregister(socket, ResourceActionType.WRITE);
        }

        private void CloseConnection(Socket socket)
        {
            socket.Close();
            _clients.Remove(socket);
            _reactor.Unregister(socket, ResourceActionType.READ);
            _reactor.Unregister(socket, ResourceActionType.WRITE);
        }
        #endregion Private

        #region Public
        /// <summary>
        /// Start the Chat server.
        /// </summary>
        public void Start()
        {
            Console.WriteLine($"Server start running on port {_port}...");
            _listener.Start();
            _reactor.Register(_listener.Server, ResourceActionType.READ, WaitConnectionsRequests);
            _reactor.Start();
        }

        /// <summary>
        /// Stop the Chat server.
        /// </summary>
        public void Stop()
        {
            _reactor.Stop();
            _listener.Stop();
            Console.WriteLine($"Server stop running on port {_port}...");
            _clients.ForEach((socketClient) => socketClient.Close());
            _reactor.Dispose();
        }
        #endregion Public
    }
}
