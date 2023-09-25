using System.Text.Json.Serialization;
using CompanyManager.Api.Extensions;
using CompanyManager.Application.Core.Commands;
using CompanyManager.Persistence.EntityMapping;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Company Manager API",
        Version = "v1",
        Description = "Company Manager API implementation"
    });

    options.UseInlineDefinitionsForEnums();
    options.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services
    .AddPersistence(builder.Configuration)
    .AddSystemLogHandling(builder.Configuration)
    .AddDomainServices();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(ICommand).Assembly));
builder.Services.AddAutoMapper(typeof(EmployeeProfile).Assembly);


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company Manager API v1");
        c.DisplayOperationId();
    });
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();