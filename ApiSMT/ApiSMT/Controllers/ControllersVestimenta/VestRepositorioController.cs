using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vestimenta.BLL;
using Vestimenta.DTO;
using System;
using System.Collections.Generic;

namespace ApiSMT.Controllers.ControllersVestimenta
{
    /// <summary>
    /// Controller VestRepositorioController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VestRepositorioController : ControllerBase
    {
        private readonly IVestRepositorioBLL _repositorio;
        private readonly IVestimentaBLL _vestimenta;
        private readonly IComprasVestBLL _compras;

        /// <summary>
        /// Construtor VestRepositorioController
        /// </summary>
        /// <param name="repositorio"></param>
        /// <param name="vestimenta"></param>
        /// <param name="compras"></param>
        public VestRepositorioController(IVestRepositorioBLL repositorio, IVestimentaBLL vestimenta, IComprasVestBLL compras)
        {
            _repositorio = repositorio;
            _vestimenta = vestimenta;
            _compras = compras;
        }

        /// <summary>
        /// Retorna um item selecionado
        /// </summary>
        /// <param name="idRepositorio"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> getRepositorio(int idRepositorio)
        {
            try
            {
                var checkRepositorio = await _repositorio.getRepositorio(idRepositorio);

                if (checkRepositorio != null)
                {
                    return Ok(new { message = "Item encontrado", result = true, repositorio = checkRepositorio });
                }
                else
                {
                    return BadRequest(new { message = "Item não encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona todos os itens que serão enviados para compras
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("repositorio/{status}")]
        public async Task<ActionResult> getRepositorioStatus(string status)
        {
            try
            {
                var repositorio = await _repositorio.getRepositorioStatus(status);
                List<object> list = new List<object>();

                if (repositorio != null)
                {
                    foreach (var item in repositorio)
                    {
                        var checkNome = await _vestimenta.getVestimenta(item.idItem);

                        list.Add(new { 
                            Id = item.id,
                            IdItem = item.idItem,
                            Nome = checkNome.nome,
                            Preco = checkNome.preco,
                            IdPedido = item.idPedido,
                            Tamanho = item.tamanho,
                            Quantidade = item.quantidade
                        });
                    }

                    return Ok(new { message = "Produtos encontrados", result = true, lista = list });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum item encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Envia itens para compra
        /// </summary>
        /// <param name="compras"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> enviarParaCompra([FromBody] VestComprasDTO compras)
        {
            try
            {
                if (compras != null)
                {
                    compras.dataCompra = DateTime.Now;

                    var insereCompra = await _compras.Insert(compras);

                    if (insereCompra != null)
                    {
                        foreach (var item in compras.itensRepositorio)
                        {
                            foreach (var idRepositorio in item.idRepositorio)
                            {
                                VestRepositorioDTO repositorio = await _repositorio.getRepositorio(idRepositorio);

                                if (repositorio != null)
                                {
                                    repositorio.enviadoCompra = "S";
                                    repositorio.dataAtualizacao = DateTime.Now;

                                    await _repositorio.Update(repositorio);
                                }
                            }
                        }

                        return Ok(new { message = "Itens Enviados para compra com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Nenhum item selecionado!!!", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
