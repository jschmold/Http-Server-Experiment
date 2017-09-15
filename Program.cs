using System;
using System.Net;

namespace HTTPTestProj {
    class Program {
        static void Main(string[] args) {
            // Create the HttpServer
            var sv = new HttpServer();
            // Add the index route. Note, the I is dropped to lower case
            sv.AddRoute("Index/", (HttpListenerRequest req, HttpListenerResponse res) =>
            {
                var OutputStream = res.OutputStream;
                var output = System.Text.Encoding.UTF8.GetBytes("Hello!");
                OutputStream.Write(output, 0, output.Length);
                res.Close();
            });
            Console.WriteLine("Initialized and ready to go.");

            while(true)
            {
                var input = Console.ReadLine();
                if(input == "quit")
                    { sv.Dispose(); Environment.Exit(0); }
            }
        }
    }
}
