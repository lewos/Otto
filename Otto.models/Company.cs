namespace Otto.models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CUIT  { get; set; }
        public string Owner { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
