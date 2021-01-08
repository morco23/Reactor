using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MorCohen;

namespace ChatApp
{
    public class ChatServer
    {
        TcpListener m_listener;
        Reactor m_reactor;
        List<Socket> m_clients;
        int m_port;

        /// <summary>
        /// Represents a chat server. Waiting for new connection, get messages and broadcast the message to all connections.
        /// </summary>
        /// <param name="port"> The port of the server.</param>
        public ChatServer(int port)
        {
            m_listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            m_reactor = new Reactor();
            m_clients = new List<Socket>();
            m_port = port;
        }

        public void WaitConnectionsRequests()
        {
            Socket clientSocket = m_listener.AcceptSocket();
            m_clients.Add(clientSocket);
            m_reactor.Register(clientSocket, ResourceActionType.READ, new ResourceHandler(GetMessageAndBroadcast, clientSocket));
        }

        public void GetMessageAndBroadcast(Socket socket)
        {
            try
            {
                string message = new StreamReader(new NetworkStream(socket)).ReadLine();
                BroadcastMassage(message, socket);
            }
            catch (IOException)
            {
                IPEndPoint endpoint = (IPEndPoint)socket.RemoteEndPoint;
                Console.WriteLine($"Client - {endpoint.Address} : {endpoint.Port} closed the connection");
                socket.Close();
                m_clients.Remove(socket);
                m_reactor.Unregister(socket, ResourceActionType.READ);
                m_reactor.Unregister(socket, ResourceActionType.WRITE);
            }
        }

        public void BroadcastMassage(string message, Socket socket)
        {
            m_clients.ForEach((socketClient) =>
            {
                if ( socketClient != socket)
                {
                    m_reactor.Register(socketClient, ResourceActionType.WRITE,
                        new ResourceHandler((socketClient) => SendMessage(message, socketClient), socketClient));
                }
            });
        }

        public void SendMessage(string message, Socket socket)
        {
            try
            {
                StreamWriter sw = new StreamWriter(new NetworkStream(socket));
                sw.WriteLine(message);
                sw.Flush();
            }
            catch (IOException)
            {
                IPEndPoint endpoint = (IPEndPoint)socket.RemoteEndPoint;
                Console.WriteLine($"Unable to send message to client on address {endpoint.Address} : {endpoint.Port} ");
                m_clients.Remove(socket);
            }
            m_reactor.Unregister(socket, ResourceActionType.WRITE);
        }

        /// <summary>
        /// Start the Chat server.
        /// </summary>
        public void Start()
        {
            Console.WriteLine($"Server start running on port {m_port}...");
            m_listener.Start();
            m_reactor.Register(m_listener.Server, ResourceActionType.READ, new ResourceHandler((socket) => WaitConnectionsRequests(), null));
            m_reactor.Start();
        }

        /// <summary>
        /// Stop the Chat server.
        /// </summary>
        public void Stop()
        {
            m_reactor.Stop();
            m_listener.Stop();
            Console.WriteLine($"Server stop running on port {m_port}...");
            m_clients.ForEach((socketClient) => socketClient.Close());
        }
    }
}
