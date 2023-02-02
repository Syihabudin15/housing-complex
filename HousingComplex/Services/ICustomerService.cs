using System.ComponentModel.DataAnnotations;
using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface ICustomerService
{
    Task<Customer> CreateCustomer(Customer payload);
    Task<Customer> GetById(string id);
    Task<Customer> GetForTransaction(string email);
}