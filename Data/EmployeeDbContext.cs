using EmployeeApi.Configuration.Models;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data;
public class EmployeeDbContext : IdentityDbContext<User>
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
    : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
    public DbSet<EmployeeModel> Employees => Set<EmployeeModel>();
}