﻿using CompanyManager.Application.Companies.AddCompany;
using CompanyManager.Domain.Companies.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace CompanyManager.Api.Controllers.Companies;

public class AddCompanyRequestExample : IExamplesProvider<AddCompanyRequest>
{
    public AddCompanyRequest GetExamples()
    {
        return new AddCompanyRequest("SpendCentric", new List<EmployeeToAdd>
        {
            new()
            {
                Email = "some.name@example.com",
                Title = EmployeeTitle.Manager
            },
            new()
            {
                Email = "some.other.name@example.com",
                Title = EmployeeTitle.Developer
            }
        });
    }
}