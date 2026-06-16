using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManager.Domain.Entities;
using StockManager.Domain.Enums;
using StockManager.Domain.Interfaces;
using System.Security.Claims;

namespace StockManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EstoqueController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMovimentacaoRepository _movimentacaoRepository;

    public EstoqueController(IProdutoRepository produtoRepository, IMovimentacaoRepository movimentacaoRepository)
    {
        _produtoRepository = produtoRepository;
        _movimentacaoRepository = movimentacaoRepository;
    }

    [HttpPost("entrada")]
    [Authorize(Roles = "Administrador,Operador")]
    public async Task<IActionResult> Entrada([FromBody] MovimentacaoDto dto)
    {
        var produto = await _produtoRepository.GetByIdAsync(dto.ProdutoId);
        if (produto == null) return NotFound("Produto não encontrado");

        produto.QuantidadeEmEstoque += dto.Quantidade;
        await _produtoRepository.UpdateAsync(produto);
        await RegistrarMovimentacao(dto, TipoMovimentacao.Entrada);
        return Ok(produto);
    }

    [HttpPost("saida")]
    [Authorize(Roles = "Administrador,Operador")]
    public async Task<IActionResult> Saida([FromBody] MovimentacaoDto dto)
    {
        var produto = await _produtoRepository.GetByIdAsync(dto.ProdutoId);
        if (produto == null) return NotFound("Produto não encontrado");
        if (produto.QuantidadeDisponivel < dto.Quantidade)
            return BadRequest("Quantidade disponível insuficiente");

        produto.QuantidadeEmEstoque -= dto.Quantidade;
        await _produtoRepository.UpdateAsync(produto);
        await RegistrarMovimentacao(dto, TipoMovimentacao.Saida);
        return Ok(produto);
    }

    [HttpPost("reserva")]
    [Authorize(Roles = "Administrador,Operador")]
    public async Task<IActionResult> Reserva([FromBody] MovimentacaoDto dto)
    {
        var produto = await _produtoRepository.GetByIdAsync(dto.ProdutoId);
        if (produto == null) return NotFound("Produto não encontrado");
        if (produto.QuantidadeDisponivel < dto.Quantidade)
            return BadRequest("Quantidade disponível insuficiente para reserva");

        produto.QuantidadeReservada += dto.Quantidade;
        await _produtoRepository.UpdateAsync(produto);
        await RegistrarMovimentacao(dto, TipoMovimentacao.Reserva);
        return Ok(produto);
    }

    [HttpPost("cancelar-reserva")]
    [Authorize(Roles = "Administrador,Operador")]
    public async Task<IActionResult> CancelarReserva([FromBody] MovimentacaoDto dto)
    {
        var produto = await _produtoRepository.GetByIdAsync(dto.ProdutoId);
        if (produto == null) return NotFound("Produto não encontrado");
        if (produto.QuantidadeReservada < dto.Quantidade)
            return BadRequest("Quantidade reservada insuficiente para cancelamento");

        produto.QuantidadeReservada -= dto.Quantidade;
        await _produtoRepository.UpdateAsync(produto);
        await RegistrarMovimentacao(dto, TipoMovimentacao.CancelamentoReserva);
        return Ok(produto);
    }

    [HttpPost("ajuste")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AjusteManual([FromBody] MovimentacaoDto dto)
    {
        var produto = await _produtoRepository.GetByIdAsync(dto.ProdutoId);
        if (produto == null) return NotFound("Produto não encontrado");

        produto.QuantidadeEmEstoque = dto.Quantidade;
        await _produtoRepository.UpdateAsync(produto);
        await RegistrarMovimentacao(dto, TipoMovimentacao.AjusteManual);
        return Ok(produto);
    }

    [HttpGet("historico")]
    public async Task<IActionResult> Historico() =>
        Ok(await _movimentacaoRepository.GetAllAsync());

    [HttpGet("historico/{produtoId}")]
    public async Task<IActionResult> HistoricoPorProduto(int produtoId) =>
        Ok(await _movimentacaoRepository.GetByProdutoIdAsync(produtoId));

    private async Task RegistrarMovimentacao(MovimentacaoDto dto, TipoMovimentacao tipo)
    {
        var usuario = User.FindFirstValue(ClaimTypes.Email) ?? "sistema";
        await _movimentacaoRepository.AddAsync(new MovimentacaoEstoque
        {
            ProdutoId = dto.ProdutoId,
            Tipo = tipo,
            Quantidade = dto.Quantidade,
            Observacao = dto.Observacao,
            UsuarioResponsavel = usuario
        });
    }
}

public record MovimentacaoDto(int ProdutoId, int Quantidade, string Observacao);