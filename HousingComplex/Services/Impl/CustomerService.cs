using HousingComplex.Entities;
using HousingComplex.Exceptions;
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

    public async Task<Customer> GetById(string id)
    {
        try
        {
            var customer = await _repository.FindById(Guid.Parse(id));
            if (customer is null) throw new NotFoundException("customer not found!");
            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Customer> GetForTransaction(string email)
    {
        try
        {
            var customer = await _repository.Find(customer => customer.UserCredential.Email.Equals(email),
                new[] { "UserCredential","Meet" });
            if (customer is null) throw new NotFoundException("customer not found!");
            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}