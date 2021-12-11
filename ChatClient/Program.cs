using System;
using System.Net;
using ChatApp;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to chat!");
            Console.WriteLine("Enter your nick name");
            string nickname = Console.ReadLine();
            int serverPort = args.Length > 0 ? int.Parse(args[0]) : 1234;
            new ChatApp.ChatClient(new IPEndPoint(IPAddress.Loopback, serverPort), nickname).Start();
        }
    }
}
