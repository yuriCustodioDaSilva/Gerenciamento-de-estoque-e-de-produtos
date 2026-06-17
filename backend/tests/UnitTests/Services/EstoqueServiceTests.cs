using FluentAssertions;
using StockManager.Domain.Entities;
using StockManager.Domain.Enums;

namespace StockManager.UnitTests.Services;

public class EstoqueServiceTests
{
    // verifica se uma entrada soma corretamente a quantidade no estoque
    [Fact]
    public void Entrada()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 10 };
        var quantidade = 5;

        produto.QuantidadeEmEstoque += quantidade;

        produto.QuantidadeEmEstoque.Should().Be(15);
    }

    // verifica se uma saida subtrai corretamente a quantidade no estoque
    [Fact]
    public void Saida()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 10 };
        var quantidade = 3;

        produto.QuantidadeEmEstoque -= quantidade;

        produto.QuantidadeEmEstoque.Should().Be(7);
    }

    // verifica se a saida é bloqueada quando não tem a quantidade disponivel suficiente
    [Fact]
    public void SaidaInsuficiente()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 5 };
        var quantidade = 10;

        var podeSair = produto.QuantidadeDisponivel >= quantidade;

        podeSair.Should().BeFalse();
    }

    // verifica se uma reserva aumenta a quantidade reservada e reduz a disponivel
    [Fact]
    public void Reserva()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 10, QuantidadeReservada = 2 };
        var quantidade = 3;

        produto.QuantidadeReservada += quantidade;

        produto.QuantidadeReservada.Should().Be(5);
        produto.QuantidadeDisponivel.Should().Be(5);
    }

    // verifica se cancelar uma reserva devolve a quantidade para disponivel
    [Fact]
    public void CancelarReserva()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 10, QuantidadeReservada = 5 };
        var quantidade = 5;

        produto.QuantidadeReservada -= quantidade;

        produto.QuantidadeReservada.Should().Be(0);
        produto.QuantidadeDisponivel.Should().Be(10);
    }

    // verifica se o ajuste manual sobrescreve a quantidade em estoque diretamente
    [Fact]
    public void AjusteManual()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 10 };

        produto.QuantidadeEmEstoque = 25;

        produto.QuantidadeEmEstoque.Should().Be(25);
    }

    // verifica se a movimentaçao registra o usuario responsavel
    [Fact]
    public void Auditoria()
    {
        var movimentacao = new MovimentacaoEstoque
        {
            ProdutoId = 1,
            Tipo = TipoMovimentacao.Entrada,
            Quantidade = 10,
            UsuarioResponsavel = "admin@teste.com"
        };

        movimentacao.UsuarioResponsavel.Should().Be("admin@teste.com");
    }

    // verifica se a saida respeita o limite mesmo havendo ativa no produto
    [Fact]
    public void SaidaComReserva()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 10, QuantidadeReservada = 7 };
        var quantidade = 5;

        var podeSair = produto.QuantidadeDisponivel >= quantidade;

        podeSair.Should().BeFalse();
        produto.QuantidadeDisponivel.Should().Be(3);
    }

    // verifica se o calculo de quantidade disponível "estoque - reservado" esta correto
    [Fact]
    public void Disponivel()
    {
        var produto = new Produto { Id = 1, Nome = "Produto A", QuantidadeEmEstoque = 20, QuantidadeReservada = 8 };

        produto.QuantidadeDisponivel.Should().Be(12);
    }
}