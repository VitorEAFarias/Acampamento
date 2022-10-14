using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.DTO.FromBody;
using ControleEPI.BLL;
using System;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe de manipulação de epi's vinculados ao colaborador
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EpiVinculoController : ControllerBase
    {
        private readonly IEpiVinculoBLL _epivinculo;
        private readonly IConUserBLL _usuario;
        private readonly IProdutosBLL _produto;
        private readonly ICategoriasBLL _categoria;
        private readonly IFornecedoresBLL _fornecedor;

        /// <summary>
        /// Construtor EpiVinculoController
        /// </summary>
        /// <param name="epivinculo"></param>
        /// <param name="usuario"></param>
        /// <param name="produto"></param>
        /// <param name="categoria"></param>
        /// <param name="fornecedor"></param>
        public EpiVinculoController(IEpiVinculoBLL epivinculo, IConUserBLL usuario, IProdutosBLL produto, ICategoriasBLL categoria, IFornecedoresBLL fornecedor)
        {
            _epivinculo = epivinculo;
            _usuario = usuario;
            _produto = produto;
            _categoria = categoria;
            _fornecedor = fornecedor;
        }

        /// <summary>
        /// Lista todos os usuários e seus respectivos epi's vinculados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<EpiVinculoDTO>> getEpiVinculo()
        {
            try
            {
                var vinculos = await _epivinculo.GetVinculos();

                if (vinculos != null)
                {
                    return Ok(new { message = "Lista encontrada", data = vinculos, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Não foi encontrado nenhum vinculo", result = false});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                
            }
            
        }

        /// <summary>
        /// Seleciona um usuario com seus epi's vinculados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EpiVinculoDTO>> getEpiVinculos(int id)
        {
            try
            {
                List<object> listaEpiVinculadas = new List<object>();

                var epiVinculos = await _epivinculo.GetUsuarioVinculo(id);

                if (epiVinculos != null)
                {
                    foreach (var item in epiVinculos)
                    {
                        var produto = await _produto.getProduto(item.idItem);

                        if(produto != null)
                        {
                            var categoria = await _categoria.getCategoria(produto.idCategoria);
                            var fornecedor = await _fornecedor.getFornecedor(produto.idFornecedor);

                            listaEpiVinculadas.Add(new
                            {
                                item.id,
                                item.idUsuario,
                                item.idItem,
                                item.dataVinculo,
                                item.ativo,
                                nome = produto.nome,
                                categoria = categoria.nome,
                                fornecedor = fornecedor.nome
                            });
                        }
                    }
                    return Ok(new { message = "Lista encontrada", data = listaEpiVinculadas, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Não foi encontrado nenhum vinculo", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Seleciona os produto ao qual fazem parte de uma categoria especifica
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("pendente/{idUsuario}")]
        public async Task<ActionResult<ProdutosDTO>> GetEpiPendente([FromRoute] int idUsuario)
        {
            try
            {
                List<object> listaEpiVinculadas = new List<object>();

                var epiVinculos = await _epivinculo.GetUsuarioVinculoStatus(idUsuario, 2);

                if (epiVinculos != null)
                {
                    foreach (var item in epiVinculos)
                    {
                        var produto = await _produto.getProduto(item.idItem);

                        if (produto != null)
                        {
                            var categoria = await _categoria.getCategoria(produto.idCategoria);
                            var fornecedor = await _fornecedor.getFornecedor(produto.idFornecedor);

                            listaEpiVinculadas.Add(new
                            {
                                item.id,
                                item.idUsuario,
                                item.idItem,
                                item.dataVinculo,
                                item.ativo,
                                nome = produto.nome,
                                ca = produto.ca,
                                quantidade = produto.quantidade,
                                categoria = categoria.nome,
                                fornecedor = fornecedor.nome
                            });
                        }
                    }
                    return Ok(new { message = "Lista encontrada", lista = listaEpiVinculadas, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Não foi encontrado nenhum vinculo", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Vincula epi(s) a um colaborador
        /// </summary>
        /// <param name="vinculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EpiVinculoDTO>> postVinculo([FromBody] VinculoDTO vinculo)
        {
            try
            {
                if (!vinculo.usuario.Equals(""))
                {
                    if (!vinculo.produto.Equals(""))
                    {                        
                        var usuario = await _usuario.GetEmp(Convert.ToInt32(vinculo.usuario));                        
                        var dataVinculo = DateTime.Now;
                        List<object> indisponivel = new List<object>();
                        List<object> disponivel = new List<object>();

                        foreach (var item in vinculo.produto)
                        {
                            var vincularProdutos = new EpiVinculoDTO();                            

                            var produto = await _produto.getProduto(item.id);

                            vincularProdutos.idUsuario = Convert.ToInt32(vinculo.usuario);

                            if (produto.quantidade >= item.quantidade)
                            {
                                produto.quantidade = produto.quantidade - item.quantidade;
                                
                                await _produto.Update(produto);

                                vincularProdutos.idItem = produto.id;
                                vincularProdutos.idUsuarioVinculo = Convert.ToInt32(vinculo.usuarioLogado);
                                vincularProdutos.dataVinculo = dataVinculo;
                                vincularProdutos.ativo = 2;

                                await _epivinculo.Insert(vincularProdutos);

                                disponivel.Add(new
                                {
                                    id = produto.id,
                                    produto = produto.nome,
                                    quantidade = produto.quantidade
                                });
                            }
                            else
                            {
                                indisponivel.Add(new
                                {
                                    id = produto.id,
                                    message = "Quantidade do " + produto.nome + "+ indisponivel no estoque",
                                    data = false
                                });
                            } 
                        }

                        return Ok(new { message = "EPI's entregues ao usuário: " + usuario.nome + " as: " + dataVinculo, data = true, indisponivel = indisponivel, disponivel = disponivel }) ;
                    }
                    else
                    {
                        return BadRequest (new { messagee = "Nenhum produto selecionado para vinculo!!!", data = false });
                    }
                }
                else
                {
                    return BadRequest (new { message = "Nenhum usuário selecionado!!!", data = false});
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza epi(s) vinculado com um colaborador
        /// </summary>
        /// <param name="id"></param>
        /// <param name="epi"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Delete(int id, [FromBody] EpiVinculoDTO epi)
        {
            try
            {
                var deletaVinculo = await _epivinculo.GetVinculo(id);

                if (epi.id == deletaVinculo.id)
                {
                    var produtoVinculo = await _epivinculo.GetProdutoVinculo(deletaVinculo.id);

                    if (produtoVinculo == null)
                    {
                        await _epivinculo.Update(epi);
                        return Ok(new { message = deletaVinculo.ativo + " Excluido com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Não é possivel excluir produto vinculado a colaborador", result = false});   
                    }                    
                }
                else
                {
                    return BadRequest(new { message = "Vinculo não encontrado", data = false });
                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        
    }
}
