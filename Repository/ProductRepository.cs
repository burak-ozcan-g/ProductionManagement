using Microsoft.EntityFrameworkCore;
using ProductionManagement.Data;
using ProductionManagement.Dto;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;

namespace ProductionManagement.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateProduct(int packageId, Product product, int units)
        {
            var productPackageEntity = _context.Packages.Where(p => p.Id == packageId).FirstOrDefault();

            var productPackage = new ProductPackage()
            {
                Package = productPackageEntity,
                Product = product,
                Units = units,
            };

            _context.Add(productPackage);
            _context.Add(product);
            return  Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public bool DeleteProducts(List<Product> products)
        {
            _context.RemoveRange(products);
            return Save();
        }

        public List<Package> GetPackagesbyProduct(int productId)
        {
            return _context.ProductPackages.Where(l => l.ProductId == productId)
                .Select(rm => rm.Package).ToList();
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Where(rm => rm.Id == id).FirstOrDefault();
        }

        public List<Product> GetProducts()
        {
            return _context.Products.OrderBy(rm => rm.Id).ToList();
        }

        public Product GetProductTrimToUpper(ProductDto productCreate)
        {
            return GetProducts().Where(c => c.Name.Trim().ToUpper() == productCreate
                .Name.TrimEnd().ToUpper()).FirstOrDefault();
        }

        public bool ProductExist(int productId)
        {
            return _context.Products.Any(l => l.Id == productId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }

        //public bool UpdateUnits(int productId)
        //{
        //    var query = _context.Products.SingleOrDefault(l => l.Id == productId)
        //        .ProductPackages.
        //}
    }
}
