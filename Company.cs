using Microsoft.EntityFrameworkCore;

namespace CompanyDirectory.Models
{
  public class Department
  {
    public int Id { get; set;}
    public string? Name { get; set;}
    public int? LocationId { get; set;}
  }

  public class Location
  {
    public int Id { get; set;}
    public string? Name { get; set;}
  }

  public class Employee
  {
    public int Id { get; set;}
    public string? FirstName { get; set;}
    public string? LastName { get; set;}
    public string? JobTitle { get; set;}
    public string? Email { get; set;}
    public int? DepartmentId { get; set;}
  }

  class CompanyDb : DbContext
  {
    public CompanyDb(DbContextOptions options) : base(options) {}
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
  }
}