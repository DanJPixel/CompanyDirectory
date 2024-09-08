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

app.MapGet("/", () => "Hello World!");

// Location Endpoints //
// get all
app.MapGet("/locations", async (CompanyDb db) => await db.Locations.ToListAsync());
// get by id
// get employee count by location id
// post location
// put location
// delete location

// Department Endpoints //
// get all
app.MapGet("/departments", async (CompanyDb db) => await db.Departments.ToListAsync());
// get by id
// get employee count by department id
// post department
// put department
// delete department

// Employee Endpoints //
// get all
app.MapGet("/employees", async (CompanyDb db) => await db.Employees.ToListAsync());
// get by id
// get all by location id
// get all by department id
// post employee
// put employee
// delete employee

app.Run();
