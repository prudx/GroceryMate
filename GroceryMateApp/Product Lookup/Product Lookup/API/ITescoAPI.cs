using System.Threading.Tasks;
using Refit;
using GroceryMate.JsonData;

namespace GroceryMate.API
{
    [Headers("Ocp-Apim-Subscription-Key: 9c3efb16d55a471781e299822b6b01be")]
    public interface ITescoAPI
    {
        //query=orange&offset=0&limit=10
        [Get("/grocery/products/?query={query}&offset={offset}&limit={limit}")]
        Task<RootObject> GetItems(string query, int offset, int limit);
    }
}