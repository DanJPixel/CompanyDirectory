using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using CompanyDirectory.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CompanyDirectory") ?? "Data Source=CompanyDirectory.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<CompanyDb>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Company Directory API",
         Description = "Managing people, departments & locations.",
         Version = "v1" });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyDirectory API V1");
   });
}

// Location Endpoints //
// get all
app.MapGet("/locations", async (CompanyDb db) => await db.Locations.ToListAsync());
// get by id
app.MapGet("/locations/{id}", async (CompanyDb db, int id) => 
{
  var location = await db.Locations.FindAsync(id);
  if (location == null) return Results.NotFound();
  return Results.Ok(location);
});
// get employee count by location id
// post location
app.MapPost("/locations", async (CompanyDb db, Location location) =>
{
  await db.Locations.AddAsync(location);
  await db.SaveChangesAsync();
  return Results.Created($"/locations/{location.Id}", location);
});
// put location
app.MapPut("/locations/{id}", async (CompanyDb db, Location updatelocation, int id) => 
{
  var location = await db.Locations.FindAsync(id);
  if (location == null) return Results.NotFound();
  location.Name = updatelocation.Name;
  await db.SaveChangesAsync();
  return Results.NoContent();
});
// delete location
app.MapDelete("/locations/{id}", async (CompanyDb db, int id) => 
{
  var location = await db.Locations.FindAsync(id);
  if (location == null) return Results.NotFound();
  db.Locations.Remove(location);
  await db.SaveChangesAsync();
  return Results.Ok();
});

// Department Endpoints //
// get all
app.MapGet("/departments", async (CompanyDb db) => await db.Departments.ToListAsync());
// get by id
app.MapGet("/departments/{id}", async (CompanyDb db, int id) => 
{
  var department = await db.Departments
                            .Where(d => d.Id == id)
                            .Join(db.Locations,
                              d => d.LocationId,
                              l => l.Id,
                              (d, l) => new {
                                d.Id,
                                d.Name,
                                LocationName = l.Name
                              })
                            .FirstOrDefaultAsync();

  if (department == null) return Results.NotFound();
  
  return Results.Ok(department);
});
// get employee count by department id
// post department
app.MapPost("/departments", async (CompanyDb db, Department department) =>
{
  await db.Departments.AddAsync(department);
  await db.SaveChangesAsync();
  return Results.Created($"/departments/{department.Id}", department);
});
// put department
app.MapPut("/departments/{id}", async (CompanyDb db, Department updatedepartment, int id) => 
{
  var department = await db.Departments.FindAsync(id);
  if (department == null) return Results.NotFound();
  department.Name = updatedepartment.Name;
  department.LocationId = updatedepartment.LocationId;
  await db.SaveChangesAsync();
  return Results.NoContent();
});
// delete department
app.MapDelete("/departments/{id}", async (CompanyDb db, int id) => 
{
  var department = await db.Departments.FindAsync(id);
  if (department == null) return Results.NotFound();
  db.Departments.Remove(department);
  await db.SaveChangesAsync();
  return Results.Ok();
});

// Employee Endpoints //
// get all
app.MapGet("/employees", async (CompanyDb db) => await db.Employees.ToListAsync());
// get by id
app.MapGet("/employees/{id}", async (CompanyDb db, int id) => 
{
  var employee = await db.Employees.FindAsync(id);
  if (employee == null) return Results.NotFound();
  return Results.Ok(employee);
});
// get all by location id
// get all by department id
// post employee
app.MapPost("/employees", async (CompanyDb db, Employee employee) => 
{
  await db.Employees.AddAsync(employee);
  await db.SaveChangesAsync();
  return Results.Created($"/employees/{employee.Id}", employee);
});
// put employee
app.MapPut("/employees/{id}", async (CompanyDb db, Employee updateemployee, int id) =>
{
  var employee = await db.Employees.FindAsync(id);
  if (employee == null) return Results.NotFound();
  employee.FirstName = updateemployee.FirstName;
  employee.LastName = updateemployee.LastName;
  employee.JobTitle = updateemployee.JobTitle;
  employee.Email = updateemployee.Email;
  employee.DepartmentId = updateemployee.Id;
  await db.SaveChangesAsync();
  return Results.NoContent();
});
// delete employee
app.MapDelete("/employees/{id}", async (CompanyDb db, int id) => 
{
  var employee = await db.Employees.FindAsync(id);
  if (employee == null) return Results.NotFound();
  db.Employees.Remove(employee);
  await db.SaveChangesAsync();
  return Results.Ok();
});

app.Run();
