using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models;

public class EmployeeModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = "";
    
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = "";
    
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Department is required")]
    [EnumDataType(typeof(Department), ErrorMessage = "Invalid Department")]
    public Department Dept { get; set; }
    
    [Required(ErrorMessage = "Salary grade is required")]
    [EnumDataType(typeof(Salary), ErrorMessage = "Invalid Salary Grade")]
    public Salary Grade { get; set; }
    [Required]
    public string Email { get; set; } = "";
    
    public string ResidentialAddress { get; set; } = "";
    
}

public enum Department
{
    Sales = 1,
    Marketing = 2,
    Finance = 3,
    IT = 4
}

public enum Salary
{
    EntryLevel = 1,
    Junior = 2,
    Senior = 3,
    Manager = 4
}