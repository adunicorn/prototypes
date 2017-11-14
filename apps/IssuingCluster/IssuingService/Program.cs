using System;
using System.Threading;
using Microsoft.Owin.Hosting;
using System.Threading.Tasks;

namespace IssuingService
{
    public class Program
    {
        public static string Version = "v35 new";
        public static string RedisMasterHostName = Environment.GetEnvironmentVariable("REDIS_MASTER_SERVICE_HOST");
        public static string RedisSlaveHostName = "localhost";
        public static string RedisPassword = "redis";
        public static string RabbitMQHostName = Environment.GetEnvironmentVariable("RABBITMQ_SERVICE_HOST");

        static void Main(string[] args)
        {
            string baseAddress = "http://*:5000/";

            using (WebApp.Start<Startup>(url: baseAddress))

            {
                Console.WriteLine("App started with address '{0}' on a machine '{1}' with OS '{2}'", baseAddress, Environment.MachineName, Environment.OSVersion);
                Console.WriteLine();
                while(true)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(500);
                    }).GetAwaiter().GetResult();
                }
            }
        }
    }
}
