namespace Shared.Application.Interfaces;

public interface IRequestHandler<in TIn, TOut>
{
    Task<TOut> Handle(TIn request, CancellationToken cancellationToken);
}
