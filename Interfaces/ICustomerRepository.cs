using ProductionManagement.Dto;
using ProductionManagement.Models;

namespace ProductionManagement.Interfaces
{
    public interface ICustomerRepository
    {
        ICollection<Customer> GetCustomers();
        Customer GetCustomer(int id);
        Customer GetCustomer(string name);
        ICollection<Package> GetPackagesACustomer(int customerId);
        Customer GetCustomerTrimToUpper(CustomerDto customerCreate);
        bool CustomerExist(int customerId);
        bool CreateCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(Customer customer);
        bool Save();
    }
}
