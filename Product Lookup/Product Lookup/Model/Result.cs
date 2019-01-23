using System.Collections.Generic;

namespace Product_Lookup.Model
{
    public class Result
    {
        public string image { get; set; }
        public string superDepartment { get; set; }
        public int tpnb { get; set; }
        public string ContentsMeasureType { get; set; }
        public string name { get; set; }
        public int UnitOfSale { get; set; }
        public List<string> description { get; set; }
        public double AverageSellingUnitWeight { get; set; }
        public string UnitQuantity { get; set; }
        public int id { get; set; }
        public double ContentsQuantity { get; set; }
        public string department { get; set; }
        public double price { get; set; }
        public double unitprice { get; set; }
    }
}