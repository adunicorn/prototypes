﻿using System;
using System.Net;
using System.Text;
using System.Web.Http;
using IssuingService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ServiceStack.Redis;

namespace IssuingService.Controllers
{
    public class CardHolderController : ApiController
    {
        public static string GetLocalIPAddress()
        {


            string str="";

            System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(str);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();
        }

        [Route("")]
        [HttpGet]
        public string HelloRoot()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("IP: {0}", GetLocalIPAddress()));
            sb.AppendLine("Serving /");
            sb.AppendLine(string.Format("IssuingService: {0}", Program.Version));

            return sb.ToString();
        }

        [Route("api/cardholders/counter")]
        [HttpGet]
        public long GetCounter()
        {
            Console.WriteLine("Serving api/cardholders/counter");
            Console.WriteLine("Connecting to Redis: {0} with password: {1}", Program.RedisSlaveHostName, Program.RedisPassword);
            try
            {
                var redis = new RedisClient(Program.RedisSlaveHostName, 6379, Program.RedisPassword);

                var counter = redis.Get<int>("counter");
                return counter;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
        }


        [Route("api/cardholders/counter/increment")]
        [HttpGet]
        public long IncrementCounter()
        {
            Console.WriteLine("Serving api/cardholders/counter");
            Console.WriteLine("Connecting to Redis: {0} with password: {1}", Program.RedisMasterHostName, Program.RedisPassword);
            try
            {
                var redis = new RedisClient(Program.RedisMasterHostName, 6379, Program.RedisPassword);

                var counter = redis.Increment("counter", 1);
                return counter;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
        }


        [Route("api/cardholder/{id}")]
        [HttpGet]
        public CardHolder Get(string id)
        {
            if (id == "1")
                return new CardHolder {ID = "1", Firstname = "Marco", Lastname = "Bernasconi"};

            var redis = new RedisClient(Program.RedisSlaveHostName, 6379, Program.RedisPassword);
            var cardHolder = redis.Get<CardHolder>("cardholder_" + id);
            if(cardHolder == null)
                throw  new HttpResponseException(HttpStatusCode.NotFound);

            return cardHolder;
        }

        [Route("api/cardholder")]
        [HttpPost]
        public void Add(CardHolder cardHolder)
        {
            Console.WriteLine("Serving [POST] /api/cardholder");
            try{
                var factory = new ConnectionFactory() { HostName = Program.RabbitMQHostName };

                using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "CardHolder", type: "topic", durable: true);

                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cardHolder));

                        channel.BasicPublish(exchange: "CardHolder",
                                             routingKey: "Add",
                                             basicProperties: null,
                                             body: body);
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
