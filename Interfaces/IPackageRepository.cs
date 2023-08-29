using ProductionManagement.Models;

namespace ProductionManagement.Interfaces
{
    public interface IPackageRepository
    {
        List<Package> GetPackages();
        Package GetPackage(int id);
        List<Product> GetProductsbyPackage(int packageId);
        bool PackageExist(int packageId);
        bool CreatePackage(Package package);
        bool UpdatePackage(Package package);
        bool DeletePackage(Package package);
        bool Save();
    }
}
