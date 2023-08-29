using ProductionManagement.Data;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;
using System;

namespace ProductionManagement.Repository
{
    public class PackageRepository : IPackageRepository
    {
        private readonly DataContext _context;

        public PackageRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePackage(Package package)
        {
            _context.Add(package);
            return Save();
        }

        public bool DeletePackage(Package package)
        {
            _context.Remove(package);
            return Save();
        }

        public Package GetPackage(int id)
        {
            return _context.Packages.Where(l => l.Id == id).FirstOrDefault();
        }

        public List<Package> GetPackages()
        {
            return _context.Packages.OrderBy(l => l.Id).ToList();
        }

        public List<Product> GetProductsbyPackage(int packageId)
        {
            return _context.ProductPackages.Where(l => l.PackageId == packageId).Select(rm => rm.Product).ToList();
        }

        public bool PackageExist(int packageId)
        {
            return _context.Packages.Any(l => l.Id == packageId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePackage(Package package)
        {
            _context.Update(package);
            return Save();
        }
    }
}
