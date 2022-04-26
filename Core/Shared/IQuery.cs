using Core.Users.Application;

namespace Core.Shared;

public interface IQuery<in TInput, out TOutput>
{
    IObservable<UserDto?> GetById(TInput id);
    IObservable<IEnumerable<MultiGetDto<UserDto>>> GetById(IEnumerable<long> id);
    IObservable<IEnumerable<TOutput>> GetAll();
}