using EmployeeApi.Data;
using EmployeeApi.Models;

namespace EmployeeApi.Services;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeDbContext _context;
    public EmployeeService(EmployeeDbContext context)
    {
        _context = context;
    }
    public void Create(EmployeeModel employee)
    {
        _context.Employees.Add(employee);
    }

    public void Delete(int id)
    {
        var empToDelete = _context.Employees.Find(id);
        if(empToDelete != null) 
        _context.Employees.Remove(empToDelete); 
    }

    public IEnumerable<EmployeeModel> GetAll()
    {
        return _context.Employees.ToList();
    }

    public EmployeeModel GetById(int id)
    {
        return _context.Employees.Find(id);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Update(EmployeeModel employee)
    {
        _context.Employees.Update(employee);
    }
}