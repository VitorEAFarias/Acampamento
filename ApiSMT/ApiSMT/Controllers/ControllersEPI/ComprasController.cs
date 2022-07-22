using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.DAL;
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
        private readonly IComprasDAL _compras;
        private readonly IPedidosDAL _pedidos;

        /// <summary>
        /// Construtor ComprasController
        /// </summary>
        /// <param name="compras"></param>
        /// <param name="pedidos"></param>
        public ComprasController(IComprasDAL compras, IPedidosDAL pedidos)
        {
            _compras = compras;
            _pedidos = pedidos;
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
        /// Função para criar um novo pedido de compra
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ComprasDTO>> criaCompra([FromBody] ComprasDTO compra)
        {
            try
            {
                if (compra != null)
                {
                    var compraInserida = await _compras.Insert(compra);

                    return Ok(new { message = "Compra inserida com sucesso!!!", result = true, data = compraInserida });
                }                
                else
                {
                    return BadRequest(new { message = "Compra não identificada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);                
            }
        }
    }
}
