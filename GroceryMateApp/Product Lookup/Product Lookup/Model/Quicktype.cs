using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GroceryMate.JsonData
{
    public partial class RootObject
    {
        [JsonProperty("uk")]
        public Uk Uk { get; set; }
    }

    public partial class Uk
    {
        [JsonProperty("ghs")]
        public Ghs Ghs { get; set; }
    }

    public partial class Ghs
    {
        [JsonProperty("products")]
        public Products Products { get; set; }
    }

    public partial class Products
    {
        [JsonProperty("input_query")]
        public string InputQuery { get; set; }

        [JsonProperty("output_query")]
        public string OutputQuery { get; set; }

        [JsonProperty("filters")]
        public Filters Filters { get; set; }

        [JsonProperty("queryPhase")]
        public string QueryPhase { get; set; }

        [JsonProperty("totals")]
        public Totals Totals { get; set; }

        [JsonProperty("config")]
        public string Config { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("suggestions")]
        public List<object> Suggestions { get; set; }
    }

    public partial class Filters
    {
    }

    public partial class Result
    {
        // changing from Uri to string
        [JsonProperty("image")]
        public string Image { get; set; }

        //[JsonProperty("superDepartment")]
        //public SuperDepartment SuperDepartment { get; set; }

        [JsonProperty("tpnb")]
        public long Tpnb { get; set; }

        //[JsonProperty("ContentsMeasureType")]
        //public ContentsMeasureType ContentsMeasureType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("UnitOfSale")]
        public long UnitOfSale { get; set; }

        [JsonProperty("description")]
        public List<string> Description { get; set; }

        [JsonProperty("AverageSellingUnitWeight")]
        public double AverageSellingUnitWeight { get; set; }

        //[JsonProperty("UnitQuantity")]
        //public UnitQuantity UnitQuantity { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("ContentsQuantity")]
        public double ContentsQuantity { get; set; }

        //[JsonProperty("department")]
        //public Department Department { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("unitprice")]
        public double Unitprice { get; set; }
    }

    public partial class Totals
    {
        [JsonProperty("all")]
        public long All { get; set; }

        [JsonProperty("new")]
        public long New { get; set; }

        [JsonProperty("offer")]
        public long Offer { get; set; }
    }

    public enum ContentsMeasureType { L, Ml };

    public enum Department { MilkButterEggs };

    public enum SuperDepartment { FreshFood };

    public enum UnitQuantity { Litre };

    public partial class RootObject
    {
        public static RootObject FromJson(string json) => JsonConvert.DeserializeObject<RootObject>(json, JsonData.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RootObject self) => JsonConvert.SerializeObject(self, JsonData.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
        {
            ContentsMeasureTypeConverter.Singleton,
            UnitQuantityConverter.Singleton,
            DepartmentConverter.Singleton,
            SuperDepartmentConverter.Singleton,
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
        };
    }

    internal class ContentsMeasureTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ContentsMeasureType) || t == typeof(ContentsMeasureType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "L":
                    return ContentsMeasureType.L;
                case "ML":
                    return ContentsMeasureType.Ml;
            }
            throw new Exception("Cannot unmarshal type ContentsMeasureType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ContentsMeasureType)untypedValue;
            switch (value)
            {
                case ContentsMeasureType.L:
                    serializer.Serialize(writer, "L");
                    return;
                case ContentsMeasureType.Ml:
                    serializer.Serialize(writer, "ML");
                    return;
            }
            throw new Exception("Cannot marshal type ContentsMeasureType");
        }

        public static readonly ContentsMeasureTypeConverter Singleton = new ContentsMeasureTypeConverter();
    }

    internal class UnitQuantityConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(UnitQuantity) || t == typeof(UnitQuantity?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "LITRE")
            {
                return UnitQuantity.Litre;
            }
            throw new Exception("Cannot unmarshal type UnitQuantity");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (UnitQuantity)untypedValue;
            if (value == UnitQuantity.Litre)
            {
                serializer.Serialize(writer, "LITRE");
                return;
            }
            throw new Exception("Cannot marshal type UnitQuantity");
        }

        public static readonly UnitQuantityConverter Singleton = new UnitQuantityConverter();
    }

    internal class DepartmentConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Department) || t == typeof(Department?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Milk, Butter & Eggs")
            {
                return Department.MilkButterEggs;
            }
            throw new Exception("Cannot unmarshal type Department");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Department)untypedValue;
            if (value == Department.MilkButterEggs)
            {
                serializer.Serialize(writer, "Milk, Butter & Eggs");
                return;
            }
            throw new Exception("Cannot marshal type Department");
        }

        public static readonly DepartmentConverter Singleton = new DepartmentConverter();
    }

    internal class SuperDepartmentConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(SuperDepartment) || t == typeof(SuperDepartment?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Fresh Food")
            {
                return SuperDepartment.FreshFood;
            }
            throw new Exception("Cannot unmarshal type SuperDepartment");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (SuperDepartment)untypedValue;
            if (value == SuperDepartment.FreshFood)
            {
                serializer.Serialize(writer, "Fresh Food");
                return;
            }
            throw new Exception("Cannot marshal type SuperDepartment");
        }

        public static readonly SuperDepartmentConverter Singleton = new SuperDepartmentConverter();
    }
}
    
