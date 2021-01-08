using System;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new ChatApp.ChatServer(12231).Start();

        }

    }
}
