namespace Core.Shared;

public interface IQuery<in TInput, out TOutput>
{
    IObservable<TOutput> GetById(TInput id);
}