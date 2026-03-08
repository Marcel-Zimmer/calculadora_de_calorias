using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Data;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class RefeicaoRepository(AppDbContext context) : RepositoryBase<Refeicao>(context), IRefeicaoRepository
    {

    }
}
