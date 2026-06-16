using StockManager.Domain.Enums;

namespace StockManager.Domain.Entities;

public class MovimentacaoEstoque
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;
    public TipoMovimentacao Tipo { get; set; }
    public int Quantidade { get; set; }
    public string Observacao { get; set; } = string.Empty;
    public string UsuarioResponsavel { get; set; } = string.Empty; // auditoria
    public DateTime RealizadoEm { get; set; } = DateTime.UtcNow;
}