namespace NUnitBooking;

public interface IUnitOfWork
{
    IQueryable<T> Query<T>();
}