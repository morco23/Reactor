using System;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new ChatApp.ChatServer(int.Parse(args[0])).Start();

        }

    }
}
