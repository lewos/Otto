using System.Text.Json.Serialization;

namespace Otto.products.DTO
{

    public class TItemsSearchDTO
    {
        public List<TItemSearchDTO> Items { get; set; }
    }

    public class TItemSearchDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public Name Name { get; set; }

        [JsonPropertyName("description")]
        public Description Description { get; set; }

        [JsonPropertyName("handle")]
        public Handle Handle { get; set; }

        [JsonPropertyName("attributes")]
        public List<object> Attributes { get; set; }

        [JsonPropertyName("published")]
        public bool Published { get; set; }

        [JsonPropertyName("free_shipping")]
        public bool FreeShipping { get; set; }

        [JsonPropertyName("requires_shipping")]
        public bool RequiresShipping { get; set; }

        [JsonPropertyName("canonical_url")]
        public string CanonicalUrl { get; set; }

        [JsonPropertyName("video_url")]
        public string VideoUrl { get; set; }

        [JsonPropertyName("seo_title")]
        public SeoTitle SeoTitle { get; set; }

        [JsonPropertyName("seo_description")]
        public SeoDescription SeoDescription { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("variants")]
        public List<Variant> Variants { get; set; }

        [JsonPropertyName("tags")]
        public string Tags { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("categories")]
        public List<object> Categories { get; set; }
    }

    public class Description
    {
        [JsonPropertyName("es")]
        public string Es { get; set; }
    }

    public class Handle
    {
        [JsonPropertyName("es")]
        public string Es { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }

        [JsonPropertyName("src")]
        public string Src { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("alt")]
        public List<object> Alt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class Name
    {
        [JsonPropertyName("es")]
        public string Es { get; set; }
    }


    public class SeoDescription
    {
        [JsonPropertyName("es")]
        public string Es { get; set; }
    }

    public class SeoTitle
    {
        [JsonPropertyName("es")]
        public string Es { get; set; }
    }

    public class Variant
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("image_id")]
        public object ImageId { get; set; }

        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("compare_at_price")]
        public string CompareAtPrice { get; set; }

        [JsonPropertyName("promotional_price")]
        public string PromotionalPrice { get; set; }

        [JsonPropertyName("stock_management")]
        public bool StockManagement { get; set; }

        [JsonPropertyName("stock")]
        public int? Stock { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

        [JsonPropertyName("width")]
        public string Width { get; set; }

        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("depth")]
        public string Depth { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("values")]
        public List<object> Values { get; set; }

        [JsonPropertyName("barcode")]
        public object Barcode { get; set; }

        [JsonPropertyName("mpn")]
        public object Mpn { get; set; }

        [JsonPropertyName("age_group")]
        public object AgeGroup { get; set; }

        [JsonPropertyName("gender")]
        public object Gender { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }


}
