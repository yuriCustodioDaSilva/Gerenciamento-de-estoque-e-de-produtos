namespace StockManager.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int QuantidadeEmEstoque { get; set; }
    public int QuantidadeReservada { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();

    public int QuantidadeDisponivel => QuantidadeEmEstoque - QuantidadeReservada;
}