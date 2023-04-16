using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data;
public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
    :base(options)
    {
    }
    public DbSet<EmployeeModel> Employees => Set<EmployeeModel>();
}