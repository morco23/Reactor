using System;
using System.Net;
using ChatApp;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter you nickname:");
            string nickname = Console.ReadLine();
            new ChatApp.ChatClient(new IPEndPoint(IPAddress.Loopback, 12231), nickname).Start();
        }
    }
}
