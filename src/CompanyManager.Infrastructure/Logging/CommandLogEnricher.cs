using CompanyManager.Application.Core.Commands;
using Serilog.Core;
using Serilog.Events;

namespace CompanyManager.Infrastructure.Logging;

public class CommandLogEnricher<TResult> : ILogEventEnricher
{
    private readonly ICommand<TResult> _command;

    public CommandLogEnricher(ICommand<TResult> command)
    {
        _command = command;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id.ToString()}")));
    }
}