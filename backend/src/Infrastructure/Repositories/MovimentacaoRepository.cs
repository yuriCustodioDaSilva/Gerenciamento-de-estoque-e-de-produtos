using Microsoft.EntityFrameworkCore;
using StockManager.Domain.Entities;
using StockManager.Domain.Interfaces;
using StockManager.Infrastructure.Data;

namespace StockManager.Infrastructure.Repositories;

public class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly AppDbContext _context;

    public MovimentacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync() =>
        await _context.Movimentacoes.Include(m => m.Produto).ToListAsync();

    public async Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoIdAsync(int produtoId) =>
        await _context.Movimentacoes
            .Where(m => m.ProdutoId == produtoId)
            .Include(m => m.Produto)
            .ToListAsync();

    public async Task AddAsync(MovimentacaoEstoque movimentacao)
    {
        await _context.Movimentacoes.AddAsync(movimentacao);
        await _context.SaveChangesAsync();
    }
}