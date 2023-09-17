using MediatR;

namespace CompanyManager.Application.Core.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{

}