﻿using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Application.Companies.AddCompany;

public class EmployeeToAdd
{
    public string? Email { get; set; }
    public EmployeeTitle? Title { get; set; }
    public Guid? Id { get; set; }
}