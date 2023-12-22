namespace Final_Project.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SaleProduct> SaleProducts { get; set; }
        public Sale()
        {
            SaleProducts = new List<SaleProduct>();

        }
    }
}
