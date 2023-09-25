namespace CompanyManager.Api.Controllers.Employees;

using CompanyManager.Domain.Companies.Employees;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

public class AddEmployeeRequestExample : IExamplesProvider<AddEmployeeRequest>
{
    public AddEmployeeRequest GetExamples()
    {
        return new AddEmployeeRequest(
            new List<Guid>
            {
                Guid.NewGuid()
            },
            EmployeeTitle.Developer,
            "some.developer@example.com");
    }
}
