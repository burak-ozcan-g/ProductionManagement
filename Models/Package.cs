namespace ProductionManagement.Models
{
    public class Package
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int Cost { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ProductPackage> ProductPackages { get; set; }
    }
}
