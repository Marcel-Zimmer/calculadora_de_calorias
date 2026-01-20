using CalculadoraCalorias.Core.Domain.Entities;
using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Repository
{
    public class RegistroFisicoRepository(AppDbContext context) : IRegistroFisicoRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<RegistroFisico> Adicionar(RegistroFisico registroFisico)
        {
            await _context.RegistroFisico.AddAsync(registroFisico);
            await _context.SaveChangesAsync();
            return registroFisico;
        }

    }
}
