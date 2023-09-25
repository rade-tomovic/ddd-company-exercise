using System.Text;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CompanyManager.Application.Core.Validation;

public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<ValidationFailure> errors = _validators.Select(v => v.Validate(request)).SelectMany(result => result.Errors)
            .Where(error => error != null).ToList();

        if (!errors.Any())
            return next();

        var errorBuilder = new StringBuilder();

        errorBuilder.AppendLine("Invalid command, reason: ");

        foreach (ValidationFailure? error in errors)
            errorBuilder.AppendLine(error.ErrorMessage);

        throw new InvalidCommandException(errorBuilder.ToString(), "");
    }
}