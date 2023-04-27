using EmployeeApi.Contracts;
using EmployeeApi.Data;
using EmployeeApi.Extensions;
using EmployeeApi.Models;
using EmployeeApi.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Repository;

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
        if (empToDelete != null)
            _context.Employees.Remove(empToDelete);
    }

    public async Task<PagedList<EmployeeModel>> GetAll(EmployeeParameters employeeParameters)
    {
        var employees = await _context.Employees
        .Where(e => e.Email != null) // (e.Email == employeeParameters.Email) && (e.ResidentialAddress == employeeParameters.ResidentialAddress))
        .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
        .Search(employeeParameters.SearchTerm)
        .Sort(employeeParameters.OrderBy)
        .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
        .Take(employeeParameters.PageSize)
        .ToListAsync();

        var count = _context.Employees.Count();
        return new PagedList<EmployeeModel>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
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