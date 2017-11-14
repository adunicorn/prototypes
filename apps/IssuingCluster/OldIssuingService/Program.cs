using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace OldIssuingService
{
    public static class Program
    {
        public static string Version = "v4";

        public static void Main(string[] args)
        {
            string baseAddress = "http://*:9100/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("App version {0} started with address '{1}' on a machine '{2}' with OS '{3}'", Program.Version, baseAddress,
                    Environment.MachineName, Environment.OSVersion);
                Console.WriteLine();
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
