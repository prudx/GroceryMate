using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using GroceryFriendService.DataObjects;
using GroceryFriendService.Models;

namespace GroceryFriendService.Controllers
{
    [Authorize]
    public class ReceiptController : TableController<Receipt>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            GroceryFriendContext context = new GroceryFriendContext();
            DomainManager = new EntityDomainManager<Receipt>(context, Request);
        }

        // GET tables/Receipt
        public IQueryable<Receipt> GetAllReceipt()
        {
            return Query(); 
        }

        // GET tables/Receipt/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Receipt> GetReceipt(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Receipt/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Receipt> PatchReceipt(string id, Delta<Receipt> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Receipt
        public async Task<IHttpActionResult> PostReceipt(Receipt item)
        {
            Receipt current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Receipt/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteReceipt(string id)
        {
             return DeleteAsync(id);
        }
    }
}
