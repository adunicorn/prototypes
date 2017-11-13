using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using IssuingService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ServiceStack.Redis;

namespace IssuingService.Controllers
{
    public class TransactionController : ApiController
    {
        public static string GetLocalIPAddress()
        {
            var str="";
            Dns.GetHostName();
            var ipEntry = System.Net.Dns.GetHostEntry(str);
            var addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();
        }

        [Route("")]
        [HttpGet]
        public Info HelloRoot()
        {
            return new Info {
                IP = GetLocalIPAddress(),
                Serving = "/",
                Service = "IssuingService",
                Version = Program.Version
            };
        }

        [Route("api/transactions/counter/increment")]
        [HttpGet]
        public long IncrementCounter()
        {
            Console.WriteLine("Serving api/transactions/counter");
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


        [Route("api/transaction/{id}")]
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage response;

            if (id == "1")
                response = Request.CreateResponse(HttpStatusCode.OK, new Transaction {id = "1", description = "Some hotel", amount = "100", currency = "CHF"});
            else
            {
                var redis = new RedisClient(Program.RedisSlaveHostName, 6379, Program.RedisPassword);
                var transaction = redis.Get<Transaction>("transaction_" + id);
                if (transaction == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);
            }

            response.Headers.Add("version", Program.Version);

            return response;
        }

        [Route("api/transaction")]
        [HttpPost]
        public string Add(Transaction transaction)
        {
            Console.WriteLine("Serving [POST] /api/transaction");
            try{
                var factory = new ConnectionFactory() { HostName = Program.RabbitMQHostName };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "Transaction", type: "topic", durable: true);

                    var json = JsonConvert.SerializeObject(transaction);
                    var body = Encoding.UTF8.GetBytes(json);
                    Console.WriteLine("Publishing the message: "  + json);
                    channel.BasicPublish(exchange: "Transaction",
                        routingKey: "Add",
                        basicProperties: null,
                        body: body);
                    return json;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }
    }
}
