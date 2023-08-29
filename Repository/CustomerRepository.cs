using ProductionManagement.Data;
using ProductionManagement.Dto;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;

namespace ProductionManagement.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        public CustomerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateCustomer(Customer customer)
        {
            _context.Add(customer);
            return Save();
        }

        public bool DeleteCustomer(Customer customer)
        {
            _context.Remove(customer);
            return Save();
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.Where(p => p.Id == id).FirstOrDefault();
        }

        public Customer GetCustomer(string name)
        {
            return _context.Customers.Where(p => p.Name == name).FirstOrDefault();
        }

        public ICollection<Customer> GetCustomers()
        {
            return _context.Customers.OrderBy(c => c.Id).ToList();
        }

        public bool CustomerExist(int customerId)
        {
            return _context.Customers.Any(c => c.Id == customerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }

        public ICollection<Package> GetPackagesACustomer(int customerId)
        {
            return _context.Packages.Where(p => p.Customer.Id == customerId).ToList();
        }

        public Customer GetCustomerTrimToUpper(CustomerDto customerCreate)
        {
            return GetCustomers().Where(c => c.Name.Trim().ToUpper() == customerCreate
            .Name.TrimEnd().ToUpper()).FirstOrDefault();
        }
    }
}
