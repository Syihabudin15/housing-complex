using System.Data.SqlTypes;
using HousingComplex.Dto.Customer;
using HousingComplex.Dto.Housing;
using HousingComplex.Dto.Meet;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;

namespace HousingComplex.Services.Impl;

public class MeetService : IMeetService
{
    private readonly IRepository<Meet> _repository;
    private readonly IPersistence _persistence;
    private readonly IHousingService _housingService;
    private readonly ICustomerService _customerService;
    private readonly IDeveloperService _developerService;

    public MeetService(IRepository<Meet> repository, IPersistence persistence, IHousingService housingService, ICustomerService customerService, IDeveloperService developerService)
    {
        _repository = repository;
        _persistence = persistence;
        _housingService = housingService;
        _customerService = customerService;
        _developerService = developerService;
    }

    public async Task<MeetResponse> CreateMeetSchedule(Meet meet)
    {
        var enteredDate = DateTime.Parse(meet.MeetDate);
        var housing = await _housingService.GetById(meet.HousingId.ToString());
        var customer = await _customerService.GetById(meet.CustomerId.ToString());
        switch (housing.OpenTime.ToLower())
        {
            case "weekend":
                if ((enteredDate.DayOfWeek == DayOfWeek.Saturday) || enteredDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    var result = await _repository.Save(meet);
                    await _persistence.SaveChangesAsync();
                    return ConvertMeetToMeetResponse(result,customer,housing);
                }

                throw new ScheduleNotFoundException("Sorry The Housing is Close at weekday, try at weekend");
            
            case "weekday":
                if ((enteredDate.DayOfWeek >= DayOfWeek.Monday) && (enteredDate.DayOfWeek <= DayOfWeek.Friday) )
                {
                    var result = await _repository.Save(meet);
                    await _persistence.SaveChangesAsync();
                    return ConvertMeetToMeetResponse(result,customer,housing);
                }
                throw new ScheduleNotFoundException("Sorry The Housing is Close at weekend, Try at weekday");
        }

        return ConvertMeetToMeetResponse(new Meet(),customer,housing);

    }

    public async Task<MeetResponse> UpdateStatusMeet(string id)
    {
        try
        {
            var result = await _repository.Find(meet => meet.Id.Equals(Guid.Parse(id)),new []{"Housing","Customer"});
            if (result is null) throw new NotFoundException("Meeting Schedule Not Found!");
            result.IsMeet = true;
            await _persistence.SaveChangesAsync();
            return ConvertMeetToMeetResponse(result,result.Customer,result.Housing);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PageResponse<MeetResponse>> GetAllSchedule(int page, int size, string email)
    {
        var developer = await _developerService.GetForMeeting(email);
        var responses = developer.Housing.Meets.Select(meet => new MeetResponse
        {
            Id = meet.Id.ToString(),
            MeetDate = meet.MeetDate,
            IsMeet = meet.IsMeet,
            Housing = new HousingResponse
            {
                Id = meet.Housing.Id.ToString(),
                Name = meet.Housing.Name,
                Address = meet.Housing.Address,
                OpenTime = meet.Housing.OpenTime,
                City = meet.Housing.City
            },
            Customer = new CustomerResponse
            {
                Id = meet.Customer.Id.ToString(),
                FisrtName = meet.Customer.FirstName,
                LastName = meet.Customer.LastName,
                City = meet.Customer.City,
                PostalCode = meet.Customer.PostalCode,
                Address = meet.Customer.Address,
                PhoneNumber = meet.Customer.PhoneNumber
            }
        }).ToList();

        var totalPage = (int)Math.Ceiling((developer.Housing.Meets.Count()) / (decimal)size);
        
        PageResponse<MeetResponse> result = new()
        {
            Content = responses,
            TotalPages = totalPage,
            TotalElement = developer.Housing.Meets.Count()
        };

        return result;
    }

    private MeetResponse ConvertMeetToMeetResponse(Meet meet, Customer customer, Housing housing)
    {
        var meetResponse = new MeetResponse
        {
            Id = meet.Id.ToString(),
            MeetDate = meet.MeetDate,
            IsMeet = meet.IsMeet,
            Housing = new HousingResponse
            {
                Id = housing.Id.ToString(),
                Name = housing.Name,
                Address = housing.Address,
                OpenTime = housing.OpenTime,
                City = housing.City
            },
            Customer = new CustomerResponse
            {
                Id = customer.Id.ToString(),
                FisrtName = customer.FirstName,
                LastName = customer.LastName,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber
            }
        };

        return meetResponse;
    }
}