using HousingComplex.Entities;
using HousingComplex.Repositories;

namespace HousingComplex.Services.Impl;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;
    private readonly IPersistence _persistence;


    public CustomerService(IRepository<Customer> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }
    
    public async Task<Customer> CreateCustomer(Customer payload)
    {
        var customer = await _repository.Save(payload);
        await _persistence.SaveChangesAsync();
        return customer;
    }
}