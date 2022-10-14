using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.BLL;
using System.Collections.Generic;
using System;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe que manipula as informações relacionadas a compras de produtos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly IComprasBLL _compras;
        private readonly IPedidosBLL _pedidos;
        private readonly IProdutosBLL _produtos;
        private readonly ILogEstoqueBLL _log;

        /// <summary>
        /// Construtor ComprasController
        /// </summary>
        /// <param name="compras"></param>
        /// <param name="pedidos"></param>
        /// <param name="produtos"></param>
        /// <param name="log"></param>
        public ComprasController(IComprasBLL compras, IPedidosBLL pedidos, IProdutosBLL produtos, ILogEstoqueBLL log)
        {
            _compras = compras;
            _pedidos = pedidos;
            _produtos = produtos;
            _log = log;
        }

        /// <summary>
        /// Lista todas as compras
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ComprasDTO>> getFornecedores()
        {
            try
            {
                var compras = await _compras.getCompras();

                if (compras != null)
                {
                    return Ok(new { message = "Compras encontradas", lista = compras, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum compra encontrada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona uma compra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ComprasDTO>> getCompra(int id)
        {
            try
            {
                var compra = await _compras.getCompra(id);

                if (compra != null)
                {
                    return Ok(new { message = "Compra encontrada: '" + compra, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Compra não encontrada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona compras por status
        /// </summary>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        [HttpGet("{idStatus}")]
        public async Task<ActionResult<ComprasDTO>> getStatusCompras(int idStatus)
        {
            try
            {
                var compra = await _compras.getCompra(idStatus);

                if (compra != null)
                {
                    return Ok(new { message = "Compras encontradas: '" + compra, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Compra não encontrada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cadastra uma nova compra
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ComprasDTO>> postCadastrarCompra([FromBody] ComprasDTO compra)
        {
            try
            {
                if (compra != null)
                {
                    var checkPedido = await _pedidos.getPedido(compra.idPedido);

                    if (checkPedido != null)
                    {
                        var novaCompra = await _compras.Insert(compra);

                        if (novaCompra != null)
                        {
                            return Ok(new { message = "Compra cadastrada com sucesso!!!", result = true });
                        }
                        else
                        {
                            return BadRequest(new { message = "Erro ao cadastrar nova compra", result = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Pedido não encontrado", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Erro ao cadastrar compra " + compra, result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Efetuar compra e atualizar estoque
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comprar"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> putRealizarCompra([FromRoute] int id, [FromBody] ComprasDTO comprar)
        {
            try
            {
                var compra = await _compras.getCompra(id);
                string mensagem = "";

                if (compra != null)
                {
                    ProdutosDTO estoque = new ProdutosDTO();
                    LogEstoqueDTO log = new LogEstoqueDTO();

                    List<object> relatorioCompra = new List<object>();

                    foreach (var item in compra.produtos)
                    {
                        var checkProduto = await _produtos.getProduto(item.idProduto);

                        if (checkProduto != null)
                        {
                            var checkEstoque = await _produtos.getProduto(item.idProduto);

                            int quantidadeEstoque = checkEstoque.quantidade + item.quantidade;

                            if (checkEstoque != null)
                            {
                                estoque.id = checkEstoque.id;
                                estoque.nome = checkEstoque.nome;
                                estoque.quantidade = quantidadeEstoque;
                                estoque.ca = checkEstoque.ca;
                                estoque.valor = checkEstoque.valor;
                                estoque.idFornecedor = checkEstoque.idFornecedor;
                                estoque.idCategoria = checkEstoque.idCategoria;

                                await _produtos.Update(estoque);

                                log.idProduto = checkEstoque.id;
                                log.idUsuario = compra.idUsuario;
                                log.de = checkEstoque.quantidade;
                                log.para = quantidadeEstoque;
                                log.quantidadeMovimentada = item.quantidade;
                                log.dataAlteracao = DateTime.Now;
                                log.retirada = false;
                                log.automatico = true;                                   

                                log = await _log.Insert(log);

                                if (log != null)
                                {
                                    mensagem = "Compra do item: '" + checkEstoque.nome + "' registrada no log com sucesso!!!";
                                }
                                else
                                {
                                    mensagem = "Erro ao inserir log da compra: '" + comprar.id + "'";
                                }
                            }
                            else
                            {
                                mensagem = "Produto não cadastrado no estoque";
                            }
                        }
                        else
                        {
                            mensagem = "Vestimenta não encontrada";
                        }
                    }

                    await _compras.Update(comprar);

                    return Ok(new { message = "Procedimento de compra realizado!!!", relatorio = relatorioCompra, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Compra não encontrada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
