using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace HTTPTestProj
{
    public delegate void RouteDelegate(HttpListenerRequest req, HttpListenerResponse res);
    public class HttpServer : IDisposable {
        private HttpListener Listener { get; set; }
        private Task RouterTask;
        public Dictionary<string, RouteDelegate> Routes { get; set; }

        public HttpServer(string host = "localhost", int port = 48435)
        {
            Listener = new HttpListener();
            Routes = new Dictionary<string, RouteDelegate>();
            Listener.Prefixes.Add($"http://{host}:{port}/");
            Listener.Start();
            RouterTask = Task.Run(() => Router());
            Routes["ERR"] = (HttpListenerRequest req, HttpListenerResponse res) =>
            {
                var output = System.Text.Encoding.UTF8.GetBytes("Sorry, that address was not found. Please try again.");
                res.OutputStream.Write(output, 0, output.Length);
                res.Close();
            };
            Console.WriteLine("Server started");
        }

        private void Router()
        {
            while(true)
            {
                var Context = (Listener).GetContext();
                var Request = Context.Request;
                var Response = Context.Response;
                var RequestedRoute = GetRoute(Request);
                if (RequestedRoute == "/") RequestedRoute = "index/";
                Console.WriteLine($"Route {RequestedRoute}");
                if (HasRoute(RequestedRoute, Routes))
                    { Task.Run(() => Routes[RequestedRoute.ToLower()](Request, Response)); }
                else
                    { Task.Run(() => Routes["ERR"](Request, Response)); }
            }
        }

        public void AddRoute(string route, RouteDelegate del)
        {
            if (HasRoute(route, Routes))
                { throw new ArgumentException("Route already exists"); }
            Routes.Add(route.ToLower(), del);
            Console.WriteLine("Added route " + route.ToLower());
        }

        public void RemoveRoute(string route)
        {
            if (HasRoute(route, Routes))
                { Routes.Remove(route.ToLower()); }
        }

        public void Dispose()
            { ((IDisposable)Listener).Dispose(); }

        public static Func<HttpListenerRequest, string> GetRoute = 
            (HttpListenerRequest req) => JoinStrings(req.Url.Segments);

        public static Func<string[], string> JoinStrings = 
            (string[] val) => String.Join(String.Empty, val);

        public static Func<string, Dictionary<string, RouteDelegate>, bool> HasRoute =
            (string rt, Dictionary<string, RouteDelegate> container) => container.ContainsKey(rt.ToLower());

    }
}
