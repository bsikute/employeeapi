using EmployeeApi.Models;

namespace EmployeeApi.Services;

public interface IEmployeeService
{
    IEnumerable<EmployeeModel> GetAll();
    EmployeeModel GetById(int id);
    void Create(EmployeeModel employee);
    void Update(EmployeeModel employee);
    void Delete(int id);
    void Save();
}