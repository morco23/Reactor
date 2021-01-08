using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp
{
    public class ChatClient
    {
        IPEndPoint m_serverAdd;
        string m_nickName;
        public ChatClient(IPEndPoint endpont, string nickName)
        {
            m_serverAdd = endpont;
            m_nickName = nickName;
        }

        /// <summary>
        /// Start the Chat client. For stop the user should enter 'exit'.
        /// </summary>
        public void Start()
        {
            TcpClient chatSocketClient = new TcpClient();
            chatSocketClient.Connect(m_serverAdd);

            StreamWriter sw = new StreamWriter(chatSocketClient.GetStream());
            StreamReader sr = new StreamReader(chatSocketClient.GetStream());
            sw.AutoFlush = true;

            sw.WriteLine($"{m_nickName} join");
            string message = "";

            Task.Run(() =>
            {
                while (!message.Equals("exit"))
                {
                    Console.WriteLine(sr.ReadLine());
                }
            });

            message = Console.ReadLine();
            while(!message.Equals("exit"))
            {
                sw.WriteLine($"{m_nickName} : {message}");
                message = Console.ReadLine();
            }            
        }
    }
}
