using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace OldIssuingService
{
    public static class Program
    {
        public static string Version = "v1";

        public static void Main(string[] args)
        {
            string baseAddress = "http://*:9100/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("App started with address '{0}' on a machine '{1}' with OS '{2}'", baseAddress,
                    Environment.MachineName, Environment.OSVersion);
                Console.WriteLine();
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}