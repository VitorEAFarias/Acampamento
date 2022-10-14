using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.BLL;
using System;
using System.Collections.Generic;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe ItensController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ItensController : ControllerBase
    {
        private readonly IItensBLL _itens;
        private readonly IProdutosBLL _produtos;

        /// <summary>
        /// Construtor ItensController
        /// </summary>
        public ItensController(IItensBLL itens, IProdutosBLL produtos)
        {
            _itens = itens;
            _produtos = produtos;
        }

        /// <summary>
        /// Insere um novo item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ItensDTO>> insereItem([FromBody] ItensDTO item)
        {
            try
            {
                if (item != null)
                {
                    var checkProduto = _produtos.getProduto(item.idProduto);

                    if (checkProduto != null)
                    {
                        ItensDTO novoItem = new ItensDTO();

                        novoItem.idProduto = Convert.ToInt32(checkProduto);
                        novoItem.validade = item.validade;
                        novoItem.descricao = item.descricao;
                        novoItem.codigoBarra = item.codigoBarra;
                        novoItem.data = item.data;

                        var insereItem = await _itens.Insert(novoItem);

                        if (insereItem != null)
                        {
                            return Ok(new { message = "Novo item inserido com sucesso!!!", result = true });
                        }
                        else
                        {
                            return BadRequest(new { message = "", result = false});
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Produto não encontrado", result = false }); 
                    }
                }
                else
                {
                    return BadRequest(new { message = "Campos inválidos", result = false});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza item
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ItensDTO>> putItem([FromBody] ItensDTO item)
        {
            try
            {
                if (item != null)
                {
                    var checkProduto = await _produtos.getProduto(item.idProduto);

                    if (checkProduto != null)
                    {
                        await _itens.Update(item);

                        return Ok(new { message = "Item atualizado com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Produto dsativado ou excluido", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Nenhum item selecionado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get da lista de todos os itens cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getItens()
        {
            try
            {
                var Itens = await _itens.getItens();
                List<object> list = new List<object>();

                if (Itens != null)
                {
                    foreach (var item in Itens)
                    {
                        var nomeProduto = await _produtos.getProduto(item.idProduto);

                        list.Add(new { 
                            
                        });
                    }
                    
                    return Ok(new { message = "Itens encontrados", lista = list, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum item encontrado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona uma categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ItensDTO>> getItem(int id)
        {
            try
            {
                var item = await _itens.getItem(id);

                if (item != null)
                {
                    return Ok(new { message = "Item encontrado: '" +item+ "'", result = true });
                }
                else
                {
                    return BadRequest(new { message = "Categoria não encontrada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
