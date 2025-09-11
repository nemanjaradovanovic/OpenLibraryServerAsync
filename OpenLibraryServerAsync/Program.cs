using System;

namespace OpenLibraryServer.P2
{
    internal static class Program
    {
        private static WebServer _server;

        private static void Main(string[] args)
        {
            const string prefix = "http://localhost:8080/";
            Console.Title = "OpenLibrary Server P2 (Tasks/Async - Minimal)";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Booting on {prefix}");

            _server = new WebServer(prefix);
            _server.Start(); // accept na klasičnoj niti; obrada zahteva async/await

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Ready. Open your browser at {prefix}");
            Console.WriteLine("Press Enter to stop...");
            Console.ReadLine();

            _server.Dispose();
        }
    }
}
