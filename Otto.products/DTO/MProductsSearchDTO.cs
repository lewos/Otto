using System.Text.Json.Serialization;

namespace Otto.products.DTO
{
    public class MProductsSearchDTO
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("body")]
        public ProductBody Body { get; set; }
    }
    public class Attribute
    {
        [JsonPropertyName("value_struct")]
        public object ValueStruct { get; set; }

        [JsonPropertyName("values")]
        public List<Value> Values { get; set; }

        [JsonPropertyName("attribute_group_id")]
        public string AttributeGroupId { get; set; }

        [JsonPropertyName("attribute_group_name")]
        public string AttributeGroupName { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value_id")]
        public string ValueId { get; set; }

        [JsonPropertyName("value_name")]
        public string ValueName { get; set; }
    }

    public class ProductBody
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("secure_thumbnail")]
        public string SecureThumbnail { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("attributes")]
        public List<Attribute> Attributes { get; set; }

        [JsonPropertyName("available_quantity")]
        public int AvailableQuantity { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("shipping")]
        public Shipping Shipping { get; set; }
    }

    public class Shipping
    {
        [JsonPropertyName("logistic_type")]
        public string LogisticType { get; set; }

        [JsonPropertyName("store_pick_up")]
        public bool StorePickUp { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("methods")]
        public List<object> Methods { get; set; }

        [JsonPropertyName("tags")]
        public List<object> Tags { get; set; }

        [JsonPropertyName("dimensions")]
        public object Dimensions { get; set; }

        [JsonPropertyName("local_pick_up")]
        public bool LocalPickUp { get; set; }

        [JsonPropertyName("free_shipping")]
        public bool FreeShipping { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("struct")]
        public object Struct { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }


}
