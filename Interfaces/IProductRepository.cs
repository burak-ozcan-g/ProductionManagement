using ProductionManagement.Dto;
using ProductionManagement.Models;

namespace ProductionManagement.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
        Product GetProduct(int id);
        List<Package> GetPackagesbyProduct(int productId);
        Product GetProductTrimToUpper(ProductDto productCreate);
        bool ProductExist(int productId);
        bool CreateProduct(int packageId, Product product, int units);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool DeleteProducts(List<Product> products);
        bool Save();
    }
}

