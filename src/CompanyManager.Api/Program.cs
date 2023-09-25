using System.Text.Json.Serialization;
using CompanyManager.Api.Configuration;
using CompanyManager.Api.Extensions;
using CompanyManager.Application.Core;
using CompanyManager.Application.Core.Commands;
using CompanyManager.Persistence.EntityMapping;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.With()
        .CreateLogger();

    builder.Host.UseSerilog(Log.Logger);

    builder.Services.AddControllers().AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

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
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();


    WebApplication app = builder.Build();
    app.UseMiddleware<CorrelationMiddleware>();
    app.UseSerilogRequestLogging();

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}