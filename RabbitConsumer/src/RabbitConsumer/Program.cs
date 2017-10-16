using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack.Redis;
using System.Data;
using Dapper;

namespace RabbitConsumer
{
    class Program
    {
        public static string RedisMasterHostName = Environment.GetEnvironmentVariable("REDIS_MASTER_SERVICE_HOST");
        public const string RedisPassword = "redis";
        private static readonly string RabbitMQHostName = Environment.GetEnvironmentVariable("RABBITMQ_SERVICE_HOST");

        static void Main(string[] args)
        {
            Console.WriteLine("Starting application");
            while (true)
                try
                {
                    Console.WriteLine("Registering a consumer");
                    RegisterConsumer();
                }
                catch
                {
                    var sleep = 2000;
                    Console.WriteLine($"Connection to RabbitMQ failed. Retrying in {sleep / 1000} seconds");
                    Thread.Sleep(sleep);
                }
        }

        private static void RegisterConsumer()
        {
            var PGSQLConnectionString =
                $"User ID=user;Password=user;Host={Environment.GetEnvironmentVariable("POSTGRESQL_SERVICE_HOST")};Port=5432;Database=issuing;";

            Console.WriteLine("Connecting to RabbitMQ hostname: {0}", RabbitMQHostName);
            var factory = new ConnectionFactory() {HostName = RabbitMQHostName};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Transaction", type: "topic", durable: true);

                var queueName = channel.QueueDeclare("Transaction.Add", true, false, false).QueueName;
                channel.QueueBind(queue: queueName,
                    exchange: "Transaction",
                    routingKey: "Add");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("Consumer received message {0}", message);

                        var transaction = JsonConvert.DeserializeObject<Transaction>(message);
                        var redis = new RedisClient(RedisMasterHostName, 6379, RedisPassword);
                        redis.SetValue("transaction_" + transaction.id, message);
                        Console.WriteLine("Stored in Redis: " + ("transaction_" + transaction.id));

                        using (var conn = OpenConnection(PGSQLConnectionString))
                        {
                            var querySQL = @"
IF(EXISTS(SELECT TOP 1 1 FROM transactions WHERE id = @id))
    INSERT INTO transactions(id, description, amount)
        VALUES(@id, @description, @amount)
ELSE
    UPDATE transactions
       SET description = @description, amount = @amount
     WHERE id = @id
";
                            conn.Query<Transaction>(querySQL,
                                new {transaction.id, transaction.description, transaction.amount});
                        }
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

        public static IDbConnection OpenConnection(string connStr)
        {
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            return conn;
        }
    }
}