using System;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new ChatApp.ChatServer(args.Length > 0 ? int.Parse(args[0]) : 1234).Start();
        }

    }
}
