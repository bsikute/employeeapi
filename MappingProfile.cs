using AutoMapper;
using EmployeeApi.DataTransferObject.Models;
using EmployeeApi.Models;

namespace EmployeeApi;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegistrationDto, User>();
        CreateMap<EmployeeModel, EmployeeDto>();
    }
}