using Core.Users.Application;

namespace Core.Shared;

public interface IQuery<in TInput, TOutput>
{
    IObservable<TOutput?> GetById(TInput id);
    IObservable<IEnumerable<TOutput>> GetAll();
    IObservable<IEnumerable<MultiGetDto<TOutput>>> GetById(IEnumerable<long> elements);
}