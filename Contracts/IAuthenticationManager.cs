using EmployeeApi.DataTransferObject.Models;

namespace EmployeeApi.Contracts;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<string> CreateToken();
}