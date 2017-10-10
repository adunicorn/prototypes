using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace IssuingService
{
    public class Program
    {
        public static string RedisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
        public static string RedisHostName = Environment.GetEnvironmentVariable("REDIS_SERVICE_HOST");
        public static string RabbitMQHostName = Environment.GetEnvironmentVariable("RABBITMQ_SERVICE_HOST");
        
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
