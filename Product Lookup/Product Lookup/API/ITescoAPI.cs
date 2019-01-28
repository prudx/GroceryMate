using System.Threading.Tasks;
using Refit;
using Product_Lookup.JsonData;

namespace Product_Lookup.API
{
    [Headers("Ocp-Apim-Subscription-Key: 9c3efb16d55a471781e299822b6b01be")]
    public interface ITescoAPI
    {
        //query=orange&offset=0&limit=10
        [Get("/grocery/products/?query={query}&offset={offset}&limit={limit}")]
        Task<RootObject> GetUsers(string query, int offset, int limit);
    }
}