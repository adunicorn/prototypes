using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace IssuingService
{
    public class Program
    {
        public const string RedisPassword = "redis";
        public const string RedisHostName = "redis";

        static void Main(string[] args)
        {
            string baseAddress = "http://*:5000/";

            using (WebApp.Start<Startup>(url: baseAddress))

            {
                Console.WriteLine("App started with address '{0}' on a machine '{1}' with OS '{2}'", baseAddress, Environment.MachineName, Environment.OSVersion);
                Console.WriteLine();
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
