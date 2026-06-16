using StockManager.Domain.Entities;

namespace StockManager.Domain.Interfaces;

public interface IMovimentacaoRepository
{
    Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoIdAsync(int produtoId);
    Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync();
    Task AddAsync(MovimentacaoEstoque movimentacao);
}