namespace ProductionManagement.Models
{
    public class ProductPackage
    {
        public int ProductId { get; set; }
        public int PackageId { get; set; }
        public int Units { get; set; }
        public Product Product { get; set; }
        public Package Package { get; set; }

    }
}
