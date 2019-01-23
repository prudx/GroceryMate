using System.Collections.Generic;

namespace Product_Lookup.Model
{
    public class Products
    {
        public string input_query { get; set; }
        public string output_query { get; set; }
        public Filters filters { get; set; }
        public string queryPhase { get; set; }
        public Totals totals { get; set; }
        public string config { get; set; }
        public List<Result> results { get; set; }
        public List<object> suggestions { get; set; }
    }
}