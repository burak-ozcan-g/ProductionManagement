namespace ProductionManagement.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Package> Packages { get; set; }
    }
}
