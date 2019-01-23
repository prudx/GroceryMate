using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;

namespace ProductAPI
{
    public static class Program
    {
        public static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //query statment
            queryString["query"] = "oranges";
            queryString["offset"] = "0";
            queryString["limit"] = "10";

            Console.WriteLine(queryString.ToString());

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9c3efb16d55a471781e299822b6b01be");

            var uri = "https://dev.tescolabs.com/grocery/products/?" + queryString;

            var response = await client.GetAsync(uri);

            //Console.WriteLine(response);

            //Console.WriteLine("");

            var response2 = await client.GetStringAsync(uri);

            //Console.WriteLine(response2);
        }
    }
}