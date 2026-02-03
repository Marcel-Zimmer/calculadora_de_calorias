namespace CalculadoraCalorias.Core.Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> Adicionar(T entidade);
        Task AdicionarLote(IEnumerable<T> entidades);
        Task<T?> ObterPorId(long id);
    }
}