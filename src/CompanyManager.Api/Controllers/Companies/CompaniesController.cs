using CompanyManager.Application.Companies.AddCompany;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CompanyManager.Api.Controllers.Companies;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(OperationId = "createCompany", Summary = "Creates new company and adds new or existing employees from request")]
    public async Task<IActionResult> CreateCompany([FromBody] AddCompanyRequest request)
    {
        Guid result = await _mediator.Send(new AddCompanyCommand(request.Name, request.Employees));

        return Created(string.Empty, result);
    }
}