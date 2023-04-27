using EmployeeApi.Models;
using EmployeeApi.RequestFeatures;

namespace EmployeeApi.Contracts;

public interface IEmployeeService
{
    Task<PagedList<EmployeeModel>> GetAll(EmployeeParameters employeeParameters);
    EmployeeModel GetById(int id);
    void Create(EmployeeModel employee);
    void Update(EmployeeModel employee);
    void Delete(int id);
    void Save();
}