using System.Text.Json.Serialization;

namespace Otto.orders.DTOs
{
    public class TOrderDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("store_id")]
        public string StoreId { get; set; }

        [JsonPropertyName("contact_name")]
        public string ContactName { get; set; }

        [JsonPropertyName("contact_phone")]
        public string ContactPhone { get; set; }

        [JsonPropertyName("contact_identification")]
        public object ContactIdentification { get; set; }

        [JsonPropertyName("shipping_min_days")]
        public object ShippingMinDays { get; set; }

        [JsonPropertyName("shipping_max_days")]
        public object ShippingMaxDays { get; set; }

        [JsonPropertyName("billing_name")]
        public string BillingName { get; set; }

        [JsonPropertyName("billing_phone")]
        public string BillingPhone { get; set; }

        [JsonPropertyName("billing_address")]
        public string BillingAddress { get; set; }

        [JsonPropertyName("billing_number")]
        public string BillingNumber { get; set; }

        [JsonPropertyName("billing_floor")]
        public string BillingFloor { get; set; }

        [JsonPropertyName("billing_locality")]
        public string BillingLocality { get; set; }

        [JsonPropertyName("billing_zipcode")]
        public string BillingZipcode { get; set; }

        [JsonPropertyName("billing_city")]
        public string BillingCity { get; set; }

        [JsonPropertyName("billing_province")]
        public string BillingProvince { get; set; }

        [JsonPropertyName("billing_country")]
        public string BillingCountry { get; set; }

        [JsonPropertyName("shipping_cost_owner")]
        public string ShippingCostOwner { get; set; }

        [JsonPropertyName("shipping_cost_customer")]
        public string ShippingCostCustomer { get; set; }

        [JsonPropertyName("coupon")]
        public List<object> Coupon { get; set; }

        [JsonPropertyName("promotional_discount")]
        public PromotionalDiscount PromotionalDiscount { get; set; }

        [JsonPropertyName("subtotal")]
        public string Subtotal { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("discount_coupon")]
        public string DiscountCoupon { get; set; }

        [JsonPropertyName("discount_gateway")]
        public string DiscountGateway { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonPropertyName("total_usd")]
        public string TotalUsd { get; set; }

        [JsonPropertyName("checkout_enabled")]
        public bool CheckoutEnabled { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("gateway")]
        public string Gateway { get; set; }

        [JsonPropertyName("gateway_id")]
        public object GatewayId { get; set; }

        [JsonPropertyName("gateway_name")]
        public string GatewayName { get; set; }

        [JsonPropertyName("shipping")]
        public string Shipping { get; set; }

        [JsonPropertyName("shipping_option")]
        public string ShippingOption { get; set; }

        [JsonPropertyName("shipping_option_code")]
        public string ShippingOptionCode { get; set; }

        [JsonPropertyName("shipping_option_reference")]
        public object ShippingOptionReference { get; set; }

        [JsonPropertyName("shipping_pickup_details")]
        public object ShippingPickupDetails { get; set; }

        [JsonPropertyName("shipping_tracking_number")]
        public object ShippingTrackingNumber { get; set; }

        [JsonPropertyName("shipping_tracking_url")]
        public object ShippingTrackingUrl { get; set; }

        [JsonPropertyName("shipping_store_branch_name")]
        public object ShippingStoreBranchName { get; set; }

        [JsonPropertyName("shipping_pickup_type")]
        public string ShippingPickupType { get; set; }

        [JsonPropertyName("shipping_suboption")]
        public List<object> ShippingSuboption { get; set; }

        [JsonPropertyName("extra")]
        public Extra Extra { get; set; }

        [JsonPropertyName("storefront")]
        public string Storefront { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("completed_at")]
        public CompletedAt CompletedAt { get; set; }

        [JsonPropertyName("next_action")]
        public string NextAction { get; set; }

        [JsonPropertyName("payment_details")]
        public PaymentDetails PaymentDetails { get; set; }

        [JsonPropertyName("attributes")]
        public List<object> Attributes { get; set; }

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("cancel_reason")]
        public object CancelReason { get; set; }

        [JsonPropertyName("owner_note")]
        public object OwnerNote { get; set; }

        [JsonPropertyName("cancelled_at")]
        public object CancelledAt { get; set; }

        [JsonPropertyName("closed_at")]
        public object ClosedAt { get; set; }

        [JsonPropertyName("read_at")]
        public object ReadAt { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("payment_status")]
        public string PaymentStatus { get; set; }

        [JsonPropertyName("gateway_link")]
        public object GatewayLink { get; set; }

        [JsonPropertyName("shipping_carrier_name")]
        public object ShippingCarrierName { get; set; }

        [JsonPropertyName("shipping_address")]
        public ShippingAddress ShippingAddress { get; set; }

        [JsonPropertyName("shipping_status")]
        public string ShippingStatus { get; set; }

        [JsonPropertyName("shipped_at")]
        public object ShippedAt { get; set; }

        [JsonPropertyName("paid_at")]
        public DateTime PaidAt { get; set; }

        [JsonPropertyName("landing_url")]
        public string LandingUrl { get; set; }

        [JsonPropertyName("client_details")]
        public ClientDetails ClientDetails { get; set; }

        [JsonPropertyName("app_id")]
        public object AppId { get; set; }
    }


    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class ClientDetails
    {
        [JsonPropertyName("browser_ip")]
        public string BrowserIp { get; set; }

        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }
    }

    public class CompletedAt
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("timezone_type")]
        public int TimezoneType { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
    }

    public class Extra
    {
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

    public class PaymentDetails
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("credit_card_company")]
        public object CreditCardCompany { get; set; }

        [JsonPropertyName("installments")]
        public string Installments { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("depth")]
        public string Depth { get; set; }

        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("compare_at_price")]
        public string CompareAtPrice { get; set; }

        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }

        [JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("free_shipping")]
        public bool FreeShipping { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

        [JsonPropertyName("width")]
        public string Width { get; set; }

        [JsonPropertyName("variant_id")]
        public string VariantId { get; set; }

        [JsonPropertyName("variant_values")]
        public List<object> VariantValues { get; set; }

        [JsonPropertyName("properties")]
        public List<object> Properties { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("barcode")]
        public object Barcode { get; set; }
    }

    public class PromotionalDiscount
    {
        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("store_id")]
        public int StoreId { get; set; }

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("total_discount_amount")]
        public string TotalDiscountAmount { get; set; }

        [JsonPropertyName("contents")]
        public List<object> Contents { get; set; }

        [JsonPropertyName("promotions_applied")]
        public List<object> PromotionsApplied { get; set; }
    }

    public class ShippingAddress
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("default")]
        public bool Default { get; set; }

        [JsonPropertyName("floor")]
        public string Floor { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("locality")]
        public string Locality { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("zipcode")]
        public string Zipcode { get; set; }

        [JsonPropertyName("customs")]
        public object Customs { get; set; }
    }


}
