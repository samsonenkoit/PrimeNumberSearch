using PrimeNumber;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumberServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string port = "8080";

            if(args.Count() >= 1)
                port = args[0];

            string uri = $"http://localhost:{port}/nearestPrime/";

            StartServer(uri);

            Console.ReadLine();
        }

        private static void StartServer(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("prefix");

            var server = new HttpListener();

            // текущая ос не поддерживается
            if (!HttpListener.IsSupported) return;

            server.Prefixes.Add(prefix);

            //запускаем север
            TryStartServer(server, prefix);

            while (server.IsListening)
            {
                HttpListenerContext context = server.GetContext();

                HttpListenerRequest request = context.Request;

                string result = "";

                try
                {
                    string param = request.QueryString.Get("number");
                    var startValue = long.Parse(param);

                    var startDt = DateTime.Now;

                    PrimeNumberSearcher searcher = new PrimeNumberSearcher(new AdaptivePrimeChecker(4, 100));
                    var prime = searcher.SearchNearestPrimeAsync(startValue).Result;

                    result = $"{prime} / time {(DateTime.Now - startDt).ToString()}";
                }
                catch(Exception e)
                {
                    result = e.ToString();
                }

                HttpListenerResponse response = context.Response;
                response.ContentType = "text/plain; charset=UTF-8";
                byte[] buffer = Encoding.UTF8.GetBytes(result);
                response.ContentLength64 = buffer.Length;

                using (Stream output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }

            }
        }

        private static void TryStartServer(HttpListener server, string prefix)
        {
            var username = Environment.GetEnvironmentVariable("USERNAME");
            var userdomain = Environment.GetEnvironmentVariable("USERDOMAIN");

            try
            {
                server.Start();
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode == 5)
                {
                    Console.WriteLine("You need to run the following command:");
                    Console.WriteLine("  netsh http add urlacl url={0} user={1}\\{2} listen=yes",
                        prefix, userdomain, username);
                }
                else
                {
                    throw;
                }
            }

        }
    }
}
