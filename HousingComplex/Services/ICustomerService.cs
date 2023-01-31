using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface ICustomerService
{
    Task<Customer> CreateCustomer(Customer payload);
}