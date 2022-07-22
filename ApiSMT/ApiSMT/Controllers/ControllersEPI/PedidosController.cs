using System.Collections.Generic;
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
        private readonly IMotivosDAL _motivos;
        private readonly IConUserDAL _conUser;

        /// <summary>
        /// Construtor PedidosController
        /// </summary>
        /// <param name="pedidos"></param>
        /// <param name="status"></param>
        /// <param name="motivos"></param>
        /// <param name="conUser"></param>
        public PedidosController(IPedidosDAL pedidos, IStatusDAL status, IMotivosDAL motivos, IConUserDAL conUser)
        {
            _pedidos = pedidos;
            _status = status;
            _motivos = motivos;
            _conUser = conUser;
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

                    Pedidos.data = pedido.data;
                    Pedidos.idUsuario = pedido.idUsuario;
                    Pedidos.descricao = pedido.descricao;
                    Pedidos.motivo = pedido.motivo;
                    Pedidos.produtos = pedido.produtos;
                    Pedidos.idStatus = pedido.idStatus;

                    var novoPedido = await _pedidos.Insert(Pedidos);

                    return Ok(new { message = "Pedido realizado com sucesso!!!", result = true, data = novoPedido });
                }
                else
                {
                    return BadRequest(new { message = "Produtos não encontrados", result = false});   
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
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<ProdutosDTO>> getPedidosUsuario([FromRoute] int idUsuario)
        {
            try
            {
                List<object> listaPedidos = new List<object>();

                var pedidos = await _pedidos.getPedidosUsuario(idUsuario);

                foreach(var item in pedidos)
                {
                    var motivo = await _motivos.getMotivo(item.motivo);
                    var status = await _status.getStatus(item.idStatus);
                    var usuario = await _conUser.GetEmp(item.idUsuario);

                    listaPedidos.Add(new
                    {
                        item.id,
                        item.data,
                        item.descricao,
                        item.produtos,
                        motivo = motivo.nome,
                        status = status.nome,
                        usuario = usuario.nome
                    });

                }

                return Ok(new { message = "Lista de pedidos encontrado", lista = pedidos, result = true });

            }
            catch (System.Exception ex)
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

                    return Ok(new { message = "Status encontrado", status = status.nome, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum status selecionado", result = false });
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
                    return Ok(new { message = "Lista encontrada", result = true, lista = todosStatus });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum statuso encontrado", result = false });
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

                return Ok( new { message = "lista encontrada", result = true, lista = pedidos });
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
