namespace ProductionManagement.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int UnitCost { get; set; }
        public ICollection<ProductPackage> ProductPackages { get; set; }

    }
}
