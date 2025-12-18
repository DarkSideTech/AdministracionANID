namespace AUT2Services.Domain.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
