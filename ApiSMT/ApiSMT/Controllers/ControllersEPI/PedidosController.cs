using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DAL;
using ControleEPI.DTO;
using System;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe de manipulação de pedidos de epi's
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosDAL _pedidos;
        private readonly IStatusDAL _status;

        /// <summary>
        /// Construtor PedidosController
        /// </summary>
        /// <param name="pedidos"></param>
        /// <param name="status"></param>
        public PedidosController(IPedidosDAL pedidos, IStatusDAL status)
        {
            _pedidos = pedidos;
            _status = status;
        }

        /// <summary>
        /// insere um pedido a ser feito
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<PedidosDTO>> insertPedido([FromBody] PedidosDTO pedido)
        {
            try
            {
                if (pedido.produtos != null)
                {
                    var Pedidos = new PedidosDTO();

                    Pedidos.data = DateTime.Now;
                    Pedidos.idUsuario = pedido.idUsuario;
                    Pedidos.descricao = pedido.descricao;
                    Pedidos.motivo = pedido.motivo;
                    Pedidos.produtos = pedido.produtos;
                    Pedidos.idStatus = pedido.idStatus;

                    var novoPedido = await _pedidos.Insert(Pedidos);

                    return Ok(new { message = "Fornecedor inserido com sucesso!!!", data = true });
                }
                else
                {
                    return BadRequest(new { message = "Produtos não encontrados", data = false});   
                }                  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }

        /// <summary>
        /// Verifica o status do pedido selecionado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("status/{id}")]
        public async Task<ActionResult<StatusDTO>> getStatus(int id)
        {
            try
            {
                if (id != 0)
                {
                    var status = await _status.getStatus(id);

                    return Ok(new { message = "Status encontrado", status = status.nome, data = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum status selecionado", data = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os pedidos e verifica seus respectivos status
        /// </summary>
        /// <returns></returns>
        [HttpGet("/status")]
        public async Task<ActionResult<StatusDTO>> getTodosStatus()
        {
            try
            {
                var todosStatus = await _status.getTodosStatus();

                if (todosStatus != null)
                {
                    return Ok(new { message = "Lista encontrada", data = true, lista = todosStatus });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum statuso encontrado", data = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                        
        }

        /// <summary>
        /// Lista todos os pedidos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> getPedidos()
        {
            try
            {
                var pedidos = await _pedidos.getPedidos();

                return Ok( new { message = "lista encontrada", data = true, lista = pedidos });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um pedido selecionado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> deletePedido(int id)
        {
            var deletaPedido = await _pedidos.getPedido(id);

            if (deletaPedido == null)
                return BadRequest(new { message = "Pedido não encontrato", data = false });

            await _pedidos.Delete(deletaPedido.id);
            return Ok(new { message = "Pedido deletado com sucesso!!!", data = true });
        }
    }
}
