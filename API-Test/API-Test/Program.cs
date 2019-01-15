using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;

namespace CSHttpClientSample
{
    static class Program
    {
        static void Main()
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9c3efb16d55a471781e299822b6b01be");

            // Request parameters
            queryString["gtin"] = "4548736003446";
            //queryString["tpnb"] = "{string}";
            //queryString["tpnc"] = "{string}";
            //queryString["catid"] = "{string}";

            //Check the format of the request string
            //Console.WriteLine(queryString.ToString());

            var uri = "https://dev.tescolabs.com/product/?" + queryString;

            //request data - success / fail etc
            var response = await client.GetAsync(uri);

            //actual product data
            var response2 = await client.GetStringAsync(uri);


            Console.WriteLine(response);
            Console.WriteLine("");
            Console.WriteLine(response2);
        }
    }
}