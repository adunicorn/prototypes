using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack.Redis;

namespace RabbitConsumer
{
    class Program
    {
        private static readonly string RabbitMQHostName = "rabbit";
        public const string RedisPassword = "redis";
        public const string RedisHostName = "redis";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting application");
            while(true)
                try
                {
                    Console.WriteLine("Registering a consumer");
                    RegisterConsumer();
                }
                catch
                {
                    var sleep = 2000;
                    Console.WriteLine(string.Format("Connection to RabbitMQ failed. Retrying in {0} seconds", sleep/1000));
                    Thread.Sleep(sleep);
                }
        }

        private static void RegisterConsumer()
        {
            Console.WriteLine("Connecting to RabbitMQ: {0}", RabbitMQHostName);
            var factory = new ConnectionFactory() { HostName = RabbitMQHostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "CardHolder", type: "topic", durable: true);

                var queueName = channel.QueueDeclare("CardHolder.Add", true, false, false).QueueName;
                channel.QueueBind(queue: queueName,
                    exchange: "CardHolder",
                    routingKey: "Add");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" Consumer received message {0}", message);

                        var cardHolder = JsonConvert.DeserializeObject<CardHolder>(message);
                        var redis = new RedisClient(RedisHostName, 6379, RedisPassword);
                        redis.SetValue("cardholder_" + cardHolder.ID, message);
                        Console.WriteLine("Stored in Redis: " + ("cardholder_" + cardHolder.ID));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        throw;
                    }
                };

                channel.BasicConsume(queue: queueName,
                    noAck: true,
                    consumer: consumer);

                Console.WriteLine("RabbitConsumer registered");
                Console.WriteLine();
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }

    public class CardHolder
    {
        public string ID { get; set; }
    }
}
