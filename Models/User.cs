using Microsoft.AspNetCore.Identity;

namespace EmployeeApi.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}