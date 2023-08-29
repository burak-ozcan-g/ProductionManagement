using ProductionManagement.Data;
using ProductionManagement.Models;

namespace ProductionManagement
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.ProductPackages.Any())
            {
                var productPackages = new List<ProductPackage>()
                {
                    new ProductPackage()
                    {
                        Product = new Product() {
                            Name = "Paper",
                            Description = "paper",
                            Weight = 100,
                            UnitCost = 100,
                        },
                        Package = new Package()
                        { 
                            CreateDate = new DateTime(2021,10,15),
                            Cost = 1000,
                            Customer = new Customer()
                            {
                                Name = "Burak Özcan",
                            }
                        },
                        Units = 10,
                    },
                    new ProductPackage()
                    {
                        Product = new Product() {
                            Name = "Yarn",
                            Description = "yarn",
                            Weight = 100,
                            UnitCost = 100,
                        },
                        Package = new Package()
                        {
                            CreateDate = new DateTime(2022,5,20),
                            Cost = 4000,
                            Customer = new Customer()
                            {
                                Name = "Burak Özcan",
                            }
                        },
                        Units = 4,
                    },
                    new ProductPackage()
                    {
                        Product = new Product() {
                            Name = "Cement",
                            Description = "cement",
                            Weight = 100,
                            UnitCost = 100,
                        },
                        Package = new Package()
                        {
                            CreateDate = new DateTime(2022,5,20),
                            Cost = 3000,
                            Customer = new Customer()
                            {
                                Name = "Burak Özcan",
                            }
                        },
                        Units = 2,
                    }
                };
                dataContext.ProductPackages.AddRange(productPackages);
                dataContext.SaveChanges();
            }               
        }
    }
}
