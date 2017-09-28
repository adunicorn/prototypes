using System;
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
        [Route("hello")]
        [HttpGet]
        public string Hello()
        {
            Console.WriteLine("Serving /hello");
            return "Hello, world!";
        }

        [Route("")]
        [HttpGet]
        public string HelloRoot()
        {
            Console.WriteLine("Serving /hello");
            return "Hello, world!";
        }

        [Route("api/cardholders/counter")]
        [HttpGet]
        public long GetCouter()
        {
            try
            {
                var redis = new RedisClient(Program.RedisHostName, 6379, Program.RedisPassword);

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

            var redis = new RedisClient(Program.RedisHostName, 6379, Program.RedisPassword);
            var cardHolder = redis.Get<CardHolder>("cardholder_" + id);
            if(cardHolder == null)
                throw  new HttpResponseException(HttpStatusCode.NotFound);

            return cardHolder;
        }

        [Route("api/cardholder")]
        [HttpPost]
        public void Add(CardHolder cardHolder)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

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
    }
}
