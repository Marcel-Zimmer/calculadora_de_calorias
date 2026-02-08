using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class AtividadeFisicaRepository(AppDbContext context) : RepositoryBase<AtividadeFisica>(context), IAtividadeFisicaRepository
    {

    }
}
