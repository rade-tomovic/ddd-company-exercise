using MediatR;

namespace CompanyManager.Application.Core.Queries;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{

}