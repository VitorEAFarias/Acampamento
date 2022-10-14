using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.BLL;
using ControleEPI.DTO;
using System;
using Microsoft.IdentityModel.Tokens;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe de manipulação de pedidos de epi's
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosBLL _pedidos;
        private readonly IPedidosStatusBLL _pedidosStatus;
        private readonly IStatusBLL _status;
        private readonly IMotivosBLL _motivos;
        private readonly IConUserBLL _conUser;
        private readonly IProdutosBLL _produtos;

        /// <summary>
        /// Construtor PedidosController
        /// </summary>
        /// <param name="pedidos"></param>
        /// <param name="status"></param>
        /// <param name="motivos"></param>
        /// <param name="conUser"></param>
        /// <param name="produtos"></param>
        /// <param name="pedidosStatus"></param>
        public PedidosController(IPedidosBLL pedidos, IPedidosStatusBLL pedidosStatus, IStatusBLL status, IMotivosBLL motivos, IConUserBLL conUser, IProdutosBLL produtos)
        {
            _pedidos = pedidos;
            _pedidosStatus = pedidosStatus;
            _status = status;
            _motivos = motivos;
            _conUser = conUser;
            _produtos = produtos;
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
        /// Get para pedidos de compras em aberto
        /// </summary>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        [HttpGet("status/{idStatus}")]
        public async Task<ActionResult<PedidosDTO>> getPedidosCompras(int idStatus)
        {
            try
            {
                var pedidosStatusCompras = await _pedidosStatus.getPedidosCompras(idStatus);

                if (!pedidosStatusCompras.IsNullOrEmpty())
                {
                    var produtosCompras = new List<object>();

                    foreach (var item in pedidosStatusCompras)
                    {
                        var pedidosCompras = await _pedidos.getPedidos(item.idPedido);

                        foreach (var itemPedidos in pedidosCompras)
                        {
                            var usuario = await _conUser.GetEmp(itemPedidos.idUsuario);

                            produtosCompras.Add(new
                            {
                                id = itemPedidos.id,
                                nome = itemPedidos.descricao,
                                usuario = usuario.nome
                            });
                        }                        
                    }

                    return Ok(new { message = "Pedido encontrado", lista = produtosCompras, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum pedido de compra encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona um pedido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidosDTO>> GetPedido(int id)
        {
            try
            {
                var pedido = await _pedidos.getPedido(id);

                if (pedido != null)
                {
                    var motivo = await _motivos.getMotivo(pedido.motivo);
                    var usuario = await _conUser.GetEmp(pedido.idUsuario);

                    var lista = new List<object>();

                    foreach (var value in pedido.produtos)
                    {
                        var query = await _produtos.getProduto(value.id);

                        lista.Add(new
                        {
                            value.id,
                            value.quantidade,
                            value.nome,
                            estoque = query.quantidade
                        });
                    }

                    var item = new
                    {
                        pedido.id,
                        pedido.data,
                        pedido.descricao,
                        produtos = lista,
                        motivo = motivo.nome,
                        usuario = usuario.nome
                    };


                    return Ok(new { message = "Pedido encontrado", pedido = item, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Pedido não encontrado", result = false });
                }
            }
            catch (System.Exception ex)
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
                    var usuario = await _conUser.GetEmp(item.idUsuario);

                    listaPedidos.Add(new
                    {
                        item.id,
                        item.data,
                        item.descricao,
                        item.produtos,
                        motivo = motivo.nome,
                        usuario = usuario.nome
                    });

                }

                return Ok(new { message = "Lista de pedidos encontrado", lista = listaPedidos, result = true });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///// <summary>
        ///// Verifica o status do pedido selecionado
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("status/{id}")]
        //public async Task<ActionResult<StatusDTO>> getStatus(int id)
        //{
        //    try
        //    {
        //        if (id != 0)
        //        {
        //            var status = await _status.getStatus(id);

        //            return Ok(new { message = "Status encontrado", status = status.nome, result = true });
        //        }
        //        else
        //        {
        //            return BadRequest(new { message = "Nenhum status selecionado", result = false });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

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
                var pedidos = await _pedidos.getTodosPedidos();

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
