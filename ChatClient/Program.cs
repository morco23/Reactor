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
            new ChatApp.ChatClient(new IPEndPoint(IPAddress.Loopback, int.Parse(args[0])), nickname).Start();
        }
    }
}
