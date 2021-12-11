using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatApp
{
    public class ChatClient
    {
        private readonly IPEndPoint _serverAdd;
        private readonly string _nickName;
        private bool _shouldRun;

        public ChatClient(IPEndPoint endpont, string nickName)
        {
            _serverAdd = endpont;
            _nickName = nickName;
        }

        /// <summary>
        /// Start the Chat client. For stop, the user should enter 'exit'.
        /// </summary>
        public void Start()
        {
            _shouldRun = true;
            TcpClient chatSocketClient = new TcpClient();
            chatSocketClient.Connect(_serverAdd);

            StreamWriter sw = new StreamWriter(chatSocketClient.GetStream());
            StreamReader sr = new StreamReader(chatSocketClient.GetStream());
            sw.AutoFlush = true;

            sw.WriteLine($"{_nickName} join");

            _ = RecieveMessagesFromTheServer(sr);

            RecieveMessagesFromTheClient(sw);
            chatSocketClient.Close();
        }

        private Task RecieveMessagesFromTheServer(StreamReader clientReader)
        {
            return Task.Run(() =>
            {
                while (_shouldRun)
                {
                    Console.WriteLine(clientReader.ReadLine());
                }
            });
        }

        private void RecieveMessagesFromTheClient(StreamWriter clientWrite)
        {
            string message = Console.ReadLine();
            while (!message.Equals("exit"))
            {
                clientWrite.WriteLine($"{_nickName} : {message}");
                message = Console.ReadLine();
            }
            clientWrite.WriteLine($"{_nickName} has left the chat.");

            _shouldRun = false;
        }
    }
}
