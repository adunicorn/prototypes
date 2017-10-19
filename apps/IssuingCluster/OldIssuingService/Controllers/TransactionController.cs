using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using OldIssuingService.Models;
using Dapper;
using Npgsql;

namespace OldIssuingService.Controllers
{
    public class TransactionController : ApiController
    {
        private readonly string _pgsqlConnectionString;

        protected TransactionController()
        {
            _pgsqlConnectionString =
                $"User ID=user;Password=user;Host={Environment.GetEnvironmentVariable("POSTGRESQL_SERVICE_HOST")};Port=5432;Database=issuing;";
        }

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
                Service = "OldIssuingService",
                Version = Program.Version
            };
        }


        [Route("api/transaction/{id}")]
        [HttpGet]
        public Transaction Get(string id)
        {
            Console.WriteLine($"Serving [GET] /api/transaction/{id}");

            if (id == "1")
                return new Transaction {id = "1", description = "Some hotel", amount = "100", currency = "CHF"};

            using (var conn = new NpgsqlConnection(_pgsqlConnectionString))
            {
                conn.Open();

                var output = conn.QueryFirstOrDefault<Transaction>("select id, description, amount, currency from transactions where id = @id", new {id});
                if(output == null)
                    throw  new HttpResponseException(HttpStatusCode.NotFound);

                return output;
            }
        }

        [Route("api/transaction")]
        [HttpPost]
        public string Add(Transaction transaction)
        {
            Console.WriteLine($"Serving [POST] /api/transaction for id '{transaction.id}'");
            try {
                using (var conn = new NpgsqlConnection(_pgsqlConnectionString))
                {
                    conn.Open();

                    const string sql = @"delete from transactions where id = @id;
                                             insert into transactions(id, description, amount, currency) values(@id, @description, @amount, @currency);";
                    conn.Query<Transaction>(sql,
                        new { transaction.id, transaction.description, transaction.amount, transaction.currency });

                    var json = JsonConvert.SerializeObject(transaction);
                    return json;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
