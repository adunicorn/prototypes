using System.Linq;
using System.Net;
using System.Web.Http;
using OldIssuingService.Models;
namespace OldIssuingService.Controllers
{
    public class CardController : ApiController
    {
        [Route("api/card/{id}")]
        [HttpGet]
        public Card Get(string id)
        {
            if (id == "1")
                return new Card {ID = "1", CardHolderId = "123", EmbossingLine = "Mr Mustermann" };

            using (var context = new EF.MiniIssuingEntities())
            {
                var card = context.Card.FirstOrDefault(x => x.ID == id);
                if(card == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return new Card
                {
                    ID = card.ID,
                    CardHolderId = card.CardHolderId,
                    EmbossingLine = card.EmbossingLine
                };
            }
        }

        [Route("api/card")]
        [HttpPost]
        public void Add(Card card)
        {
            using (var context = new EF.MiniIssuingEntities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var c = context.Card.FirstOrDefault(x => x.ID == card.ID);
                        if (c == null)
                        {
                            c = new EF.Card
                            {
                                ID = card.ID,
                                CardHolderId = card.CardHolderId,
                                EmbossingLine = card.EmbossingLine
                            };
                            context.Card.Add(c);
                        }
                        else
                        {
                            c.CardHolderId = card.CardHolderId;
                            c.EmbossingLine = card.EmbossingLine;
                        }

                        var log = new EF.Log();
                        log.Message = "Updated card " + card.ID;
                        context.Log.Add(log);

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