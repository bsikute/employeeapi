using EmployeeApi.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace EmployeeApi.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<EmployeeModel> FilterEmployees(this IQueryable<EmployeeModel> employees, uint minAge, uint maxAge) =>
    employees.Where(e => (e.Email != null && e.ResidentialAddress != null));
    public static IQueryable<EmployeeModel> Search(this IQueryable<EmployeeModel> employees, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employees;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return employees.Where(e => e.FirstName.ToLower().Contains(lowerCaseTerm));
    }
    public static IQueryable<EmployeeModel> Sort(this IQueryable<EmployeeModel> employees, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(e => e.FirstName.ToLower()); var orderParams = orderByQueryString.Trim().Split(',');

        var propertyInfos = typeof(EmployeeModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param)) continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            
            if (objectProperty == null) continue;

            var direction = param.EndsWith(" desc") ? "descending" : "ascending"; orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        if (string.IsNullOrWhiteSpace(orderQuery))
            return employees.OrderBy(e => e.FirstName);
        return employees.OrderBy(orderQuery);
    }
}