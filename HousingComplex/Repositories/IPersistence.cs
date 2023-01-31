namespace HousingComplex.Repositories
{
    public interface IPersistence
    {
        Task SaveChangesAsync();
        Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func);
    }
}
