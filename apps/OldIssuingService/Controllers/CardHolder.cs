using System.Linq;
using System.Net;
using System.Web.Http;
using CardHolder = OldIssuingService.Models.CardHolder;

namespace OldIssuingService.Controllers
{
    public class CardHolderController : ApiController
    {
        [Route("api/cardholder/{id}")]
        [HttpGet]
        public CardHolder Get(string id)
        {
            if (id == "1")
                return new CardHolder {ID = "1", Firstname = "Marco", Lastname = "Bernasconi"};

            using (var context = new EF.MiniIssuingEntities())
            {
                var cardHolder = context.CardHolder.FirstOrDefault(x => x.ID == id);
                if(cardHolder == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return new CardHolder
                {
                    ID = cardHolder.ID,
                    Firstname = cardHolder.Name,
                    Lastname = cardHolder.Lastname
                };
            }
        }

        [Route("api/cardholder")]
        [HttpPost]
        public void Add(CardHolder cardHolder)
        {
            using (var context = new EF.MiniIssuingEntities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var ch = context.CardHolder.FirstOrDefault(x => x.ID == cardHolder.ID);
                        if (ch == null)
                        {
                            ch = new OldIssuingService.EF.CardHolder
                            {
                                ID = cardHolder.ID,
                                Name = cardHolder.Firstname,
                                Lastname = cardHolder.Lastname
                            };
                            context.CardHolder.Add(ch);
                        }
                        else
                        {
                            ch.Name = cardHolder.Firstname;
                            ch.Lastname = cardHolder.Lastname;
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}