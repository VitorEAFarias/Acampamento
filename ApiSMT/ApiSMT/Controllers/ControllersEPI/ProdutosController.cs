using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.DAL;
using System;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe ProdutosController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosDAL _produtos;
        private readonly IFornecedoresDAL _fornecedor;
        private readonly ICategoriasDAL _categoria;
        private readonly ILogEstoqueDAL _logEstoque;
        private readonly IConUserDAL _usuario;
        private readonly IEpiVinculoDAL _vinculo;

        /// <summary>
        /// Construtor de ProdutosController
        /// </summary>
        /// <param name="produto"></param>
        /// <param name="fornecedor"></param>
        /// <param name="categoria"></param>
        /// <param name="logEstoque"></param>
        /// <param name="usuario"></param>
        /// <param name="vinculo"></param>
        public ProdutosController(IProdutosDAL produto, IFornecedoresDAL fornecedor, ICategoriasDAL categoria, ILogEstoqueDAL logEstoque, IConUserDAL usuario, IEpiVinculoDAL vinculo)
        {
            _produtos = produto;
            _fornecedor = fornecedor;
            _categoria = categoria;
            _logEstoque = logEstoque;
            _usuario = usuario;
            _vinculo = vinculo;
        }

        /// <summary>
        /// Lista todos os produtos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getProdutos()
        {
            try
            {
                List<object> listaProdutos = new List<object>();
                var produtos = await _produtos.getProdutos();

                foreach (var item in produtos)
                {
                    var fornecedor = await _fornecedor.getFornecedor(item.idFornecedor);
                    var categoria = await _categoria.getCategoria(item.idCategoria);

                    listaProdutos.Add(new
                    {
                        id = item.id,
                        nome = item.nome,
                        quantidade = item.quantidade,
                        ca = item.ca,
                        valor = item.valor,
                        fornecedor = fornecedor.nome,
                        categoria = categoria.nome
                    });
                }

                return Ok(new { message = "Lista encontrada", lista = listaProdutos, result = true });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona um produto ao qual é recebido um id em especifico por parametro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutosDTO>> GetProduto(int id)
        {
            try
            {
                if (!id.Equals(""))
                {
                    var produto = await _produtos.getProduto(id);

                    if (!produto.Equals(""))
                    {
                        var fornecedor = await _fornecedor.getFornecedor(produto.idFornecedor);

                        if (!fornecedor.Equals(""))
                        {
                            var categoria = await _categoria.getCategoria(produto.idCategoria);

                            if (!categoria.Equals(""))
                            {
                                return Ok(new { message = "Produto encontrado", categoria = categoria, fornecedor = fornecedor, produto = produto, result = true });
                            }
                            else
                            {
                                return BadRequest(new { message = "Produto não pertence a nenhuma categoria", result = false});
                            }
                        }
                        else
                        {
                            return BadRequest(new { message = "Fornecedor não encontrado", result = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Produto não encontrado", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Nenhum produto selecionado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Insere um novo produto na tabela enviando por parametro o id do usuario que inseriu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produto"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<ProdutosDTO>> PostProduto([FromRoute]int id, [FromBody] ProdutosDTO produto)
        {
            try
            {
                if (produto != null)
                {
                    var checkProduto = await _produtos.getNomeProduto(produto.nome);

                    if (checkProduto != null)
                    {
                        return BadRequest(new { message = "Ja existe um produto chamado: " + produto.nome, result = false });
                    }
                    else
                    {
                        var usuario = await _usuario.GetEmp(id);

                        if (usuario != null)
                        {
                            var novoLog = new LogEstoqueDTO();
                            var novoProduto = await _produtos.Insert(produto);

                            if (novoProduto != null)
                            {
                                novoLog.idProduto = produto.id;
                                novoLog.idUsuario = usuario.id;
                                novoLog.de = 0;
                                novoLog.para = produto.quantidade;
                                novoLog.quantidadeMovimentada = produto.quantidade;
                                novoLog.dataAlteracao = DateTime.Now;
                                //inserir produto = true; retirar produto = false
                                novoLog.retirada = true;
                                //inserir automatico = true; inserir manual = false
                                novoLog.automatico = false;

                                var log = await _logEstoque.Insert(novoLog);

                                if (log != null)
                                {
                                    return Ok(new { message = "O " + produto.nome + " foi inserido com sucesso!!!", result = true });
                                }
                                else
                                {
                                    return BadRequest(new { message = "Erro ao registrar log", result = false });
                                }
                            }
                            else
                            {
                                return BadRequest(new { message = "Erro ao inserir produto", result = false });
                            }
                        }
                        else
                        {
                            return BadRequest(new { message = "Nenhum usuário encontrado", result = false });
                        }                        
                    }                   
                }
                else
                {
                    return BadRequest(new { message = "Campos obrigatórios não preenchidos", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Deleta um produto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var deletaProduto = await _produtos.getProduto(id);

                if (deletaProduto == null)
                {
                    return BadRequest(new { message = "Produto não encontrado", data = false });
                }
                else
                {
                    var produtoVinculado = await _vinculo.GetVinculo(deletaProduto.id);

                    if (produtoVinculado == null)
                    {
                        await _produtos.Delete(deletaProduto.id);

                        return Ok(new { message = "Produto excluido com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Não é possivel excluir um produto vinculado a um colaborador", result = false });
                    }                    
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Atualiza a quantidade de um produto existente no estoque
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="quantidade"></param>
        /// <param name="produto"></param>
        /// <returns></returns>
        [HttpPut("quantidade")]
        public async Task<ActionResult> insereQuantidade(int idUsuario, int quantidade, [FromBody] ProdutosDTO produto)
        {
            try
            {
                if (!quantidade.Equals(0))
                {
                    var usuario = await _usuario.GetEmp(idUsuario);

                    if (usuario != null)
                    {
                        var quantidadeAtual = produto.quantidade + quantidade;
                                               
                        produto.quantidade = quantidadeAtual;
                        
                        await _produtos.Update(produto);
                        var log = new LogEstoqueDTO();

                        log.idProduto = produto.id;
                        log.idUsuario = usuario.id;
                        log.de = produto.quantidade - quantidade;
                        log.para = quantidadeAtual;
                        log.quantidadeMovimentada = quantidade;
                        log.dataAlteracao = DateTime.Now;
                        //inserir produto = true; retirar produto = false
                        log.retirada = true;
                        //inserir automatico = true; inserir manual = false
                        log.automatico = false;

                        var novoLog = await _logEstoque.Insert(log);

                        if (novoLog != null)
                        {
                            return Ok(new { message = "Produto inserido com sucesso!!!", result = true });
                        }
                        else
                        {
                            return BadRequest(new { message = "Erro ao adicionar log" });
                        }                        
                    }
                    else
                    {
                        return BadRequest(new { message = "Erro ao atualizar produto", result = false });
                    }
                }
                else
                {  
                    await _produtos.Update(produto);

                    return Ok(new { message = "Produto atualizado com sucesso!!!", result = true });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
        
        /// <summary>
        /// Atualiza os dados de um produto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduto([FromRoute]int id, [FromBody] ProdutosDTO produto)
        {
            try
            {
                var user = await _usuario.GetEmp(id);

                if (user != null)
                {
                    if (produto.id != 0)
                    {
                        await _produtos.Update(produto);

                        return Ok(new { message = produto.nome + " Atualizado com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Nenhum produto encontrado!!!", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Usuário não encontrado!!!", result = false });   
                }                
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
