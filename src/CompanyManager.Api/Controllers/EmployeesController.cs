using CompanyManager.Application.Employees.AddEmployee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CompanyManager.Api.Controllers;

[Route("api/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(OperationId = "addEmployee", Summary = "Adds new employee to a list of companies")]
    public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeRequest request)
    {
        Guid result = await _mediator.Send(new AddEmployeeCommand(request.CompanyIds, request.Title, request.Email));

        return Created(string.Empty, result);
    }
}