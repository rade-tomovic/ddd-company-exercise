namespace CompanyManager.Application.Core.Commands;

public abstract class CommandBase<TResult> : ICommand<TResult>
{
    public Guid Id { get; }

    protected CommandBase()
    {
        Id = Guid.NewGuid();
    }
}