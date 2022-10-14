using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vestimenta.BLL;
using Vestimenta.DTO;
using ControleEPI.BLL;
using System;
using System.Collections.Generic;
using ControleEPI.DTO.E_Mail;

namespace ApiSMT.Controllers.ControllersVestimenta
{
    /// <summary>
    /// Classe de pedidos de vestimenta
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VestPedidosController : ControllerBase
    {
        private readonly IPedidosVestBLL _pedidosVest;
        private readonly IConUserBLL _usuario;
        private readonly IStatusVestBLL _status;
        private readonly IEstoqueBLL _estoque;
        private readonly IVestRepositorioBLL _repositorio;
        private readonly IVestItemVinculoBLL _itemVinculo;
        private readonly ILogBLL _log;
        private readonly IMailServiceBLL _mail;

        /// <summary>
        /// Construtor de Pedidos
        /// </summary>
        /// <param name="pedidosVest"></param>
        /// <param name="usuario"></param>
        /// <param name="status"></param>
        /// <param name="estoque"></param>
        /// <param name="repositorio"></param>
        /// <param name="itemVinculo"></param>
        /// <param name="log"></param>
        /// <param name="mail"></param>
        public VestPedidosController(IPedidosVestBLL pedidosVest, IConUserBLL usuario, IStatusVestBLL status, IEstoqueBLL estoque, 
            IVestRepositorioBLL repositorio, IVestItemVinculoBLL itemVinculo, ILogBLL log, IMailServiceBLL mail)
        {
            _pedidosVest = pedidosVest;
            _usuario = usuario;
            _status = status;
            _estoque = estoque;
            _repositorio = repositorio;
            _itemVinculo = itemVinculo;
            _log = log;
            _mail = mail;
        }

        /// <summary>
        /// Insere um novo pedido
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<VestPedidosDTO>> postPedido([FromBody] VestPedidosDTO pedido)
        {
            try
            {
                var checkUsuario = await _usuario.GetEmp(pedido.idSupervisor);

                if (checkUsuario != null)
                {
                    if (pedido != null)
                    {
                        pedido.dataPedido = DateTime.Now;

                        var novaPedido = await _pedidosVest.Insert(pedido);

                        EmailRequestDTO email = new EmailRequestDTO();
                        var getEmail = await _usuario.getEmail(checkUsuario.id);

                        email.EmailDe = getEmail.valor;
                        email.EmailPara = "vitor.alves@reisoffice.com.br";
                        //email.EmailPara = "rh@reisoffice.com.br";
                        email.Conteudo = "Pedido efetuado pelo supervisor '"+checkUsuario.nome+"'";
                        email.Assunto = "Novo pedido de vestimenta";

                        await _mail.SendEmailAsync(email);

                        return Ok(new { message = "Pedido feito com sucesso!!!", result = true, data = novaPedido });
                    }
                    else
                    {
                        return BadRequest(new { message = "Erro ao efetuar pedido " + pedido, result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Supervisor não encontrado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza status do pedido e do item
        /// </summary>
        /// <param name="pedidoItem"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> atualizaStatusPedidoItem([FromBody] VestPedidosDTO pedidoItem)
        {
            try
            {
                VestPedidosDTO checkPedido = await _pedidosVest.getPedido(pedidoItem.id);

                if (checkPedido != null)
                {
                    List<ItemDTO> listaItens = new List<ItemDTO>();

                    foreach (var item in pedidoItem.item)
                    {
                        if (item.status == 5)
                        {
                            var getEstoque = await _estoque.getItemExistente(item.id, item.tamanho);

                            VestRepositorioDTO repositorio = new VestRepositorioDTO();

                            repositorio.idItem = item.id;
                            repositorio.idPedido = pedidoItem.id;
                            repositorio.enviadoCompra = "N";
                            repositorio.dataAtualizacao = DateTime.Now;
                            repositorio.tamanho = item.tamanho;
                            repositorio.quantidade = item.quantidade - getEstoque.quantidade;

                            listaItens.Add(new ItemDTO {
                                id = item.id,
                                nome = item.nome,
                                tamanho = item.tamanho,
                                quantidade = item.quantidade,
                                status = 5,
                                dataAlteracao = DateTime.Now
                            });

                            var insereItens = await _repositorio.Insert(repositorio);                                
                            
                        }
                        else if (item.status == 4)
                        {
                            VestEstoqueDTO getEstoque = await _estoque.getItemExistente(item.id, item.tamanho);
                            VestItemVinculoDTO liberadoVinculo = new VestItemVinculoDTO();

                            getEstoque.quantidade = getEstoque.quantidade - item.quantidade;
                            getEstoque.tamanho = item.tamanho;
                            getEstoque.dataAlteracao = DateTime.Now;
                            getEstoque.quantidadeUsado = item.quantidade;

                            await _estoque.Update(getEstoque);

                            VestLogDTO log = new VestLogDTO();

                            log.data = DateTime.Now;
                            log.idUsuario = pedidoItem.idUsuarioAlteracao;
                            log.idItem = item.id;
                            log.quantidadeAnt = getEstoque.quantidade;
                            log.quantidadeDep = getEstoque.quantidade - item.quantidade;
                            log.tamanho = item.tamanho;

                            var insereLog = await _log.Insert(log);

                            liberadoVinculo.nome = item.nome;
                            liberadoVinculo.tamanho = item.tamanho;
                            liberadoVinculo.quantidade = item.quantidade;
                            liberadoVinculo.status = item.status;
                            liberadoVinculo.dataAlteracao = DateTime.Now;
                            liberadoVinculo.idItem = item.id;
                            liberadoVinculo.idPedido = checkPedido.id;

                            var insereItemVinculo = await _itemVinculo.Insert(liberadoVinculo);

                            item.dataAlteracao = DateTime.Now;
                            listaItens.Add(item);
                        }
                    }

                    pedidoItem.item = listaItens;

                    await _pedidosVest.Update(pedidoItem);

                    return Ok(new { message = "Status Atualizado com sucesso!!!", result = true });
                }
                else
                {
                    return BadRequest(new { message = "Pedido não encontrado!!!", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista pedidos por usuarios
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult> getPedidosUsuario(int idUsuario)
        {
            try
            {
                var emp = await _usuario.GetEmp(idUsuario);

                if (emp != null)
                {
                    var pedidosUsuario = await _pedidosVest.getPedidosUsuarios(idUsuario);

                    List<object> lista = new List<object>();

                    foreach (var item in pedidosUsuario)
                    {
                        var status = await _status.getStatus(item.status);

                        lista.Add(new
                        {
                            Id = item.id,
                            Nome = emp.nome,
                            Pedido = item.item,
                            status = status.nome,
                            DataPedido = item.dataPedido
                        });
                    }

                    return Ok(new { message = "lista encontrada", result = true, lista = lista });
                }
                else
                {
                    var pedidos = await _pedidosVest.getPedidos();

                    List<object> lista = new List<object>();

                    foreach (var item in pedidos)
                    {
                        var status = await _status.getStatus(item.status);

                        lista.Add(new
                        {
                            Id = item.id,
                            Nome = emp.nome,
                            Pedido = item.item,
                            status = status.nome,
                            DataPedido = item.dataPedido
                        });
                    }

                    return Ok(new { message = "lista encontrada", result = true, lista = lista });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista pedidos por status
        /// </summary>
        /// <returns></returns>
        [HttpGet("status/{idStatus}")]
        public async Task<ActionResult> getPedidosStatus(int idStatus)
        {
            try
            {
                var pedidos = await _pedidosVest.getPedidosStatus(idStatus);

                if (pedidos != null)
                {
                    List<object> lista = new List<object>();

                    foreach (var item in pedidos)
                    {
                        var emp = await _usuario.GetEmp(item.idSupervisor);

                        var status = await _status.getStatus(idStatus);

                        if (emp != null)
                        {
                            lista.Add(new
                            {
                                Id = item.id,
                                Nome = emp.nome,
                                Pedido = item.item,
                                status = status.nome,
                                DataPedido = item.dataPedido
                            });
                        }
                        else
                        {
                            return BadRequest(new { message = "Supervisor não encontrado!!!", result = false });
                        }
                    }

                    return Ok(new { message = "lista encontrada", result = true, lista = lista });
                }
                else
                {
                    var todosPedidos = await _pedidosVest.getPedidos();

                    List<object> lista = new List<object>();

                    foreach (var item in todosPedidos)
                    {
                        var emp = await _usuario.GetEmp(item.idSupervisor);
                        var status = await _status.getStatus(item.status);

                        if (emp != null)
                        {
                            lista.Add(new
                            {
                                Id = item.id,
                                Nome = emp.nome,
                                Pedido = item.item,
                                status = status.nome,
                                DataPedido = item.dataPedido
                            });
                        }
                        else
                        {
                            return BadRequest(new { message = "Supervisor não encontrado!!!", result = false });
                        }
                    }

                    return Ok(new { message = "lista encontrada", result = true, lista = lista });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Pedidos liberados para vinculo 
        /// </summary>
        /// <returns></returns>
        [HttpGet("vinculo/{idPedido}")]
        public async Task<ActionResult<VestPedidosDTO>> getLiberadoVinculo(int idPedido)
        {
            try
            {
                var getPedidos = await _pedidosVest.getPedido(idPedido);
                var pedidosVinculo = string.Empty;
                List<ItemDTO> itensLiberadosVinculo = new List<ItemDTO>();

                if (getPedidos != null)
                {
                    foreach (var item in getPedidos.item)
                    {
                        if (item.status == 4)
                        {
                            itensLiberadosVinculo.Add(new ItemDTO {
                                id = item.id,
                                nome = item.nome,
                                tamanho = item.tamanho,
                                quantidade = item.quantidade,
                                status = item.status,
                                dataAlteracao = item.dataAlteracao
                            });
                        }
                    }

                    if (itensLiberadosVinculo != null)
                    {
                        return Ok(new { message = "Itens encontrados!!!", itens = itensLiberadosVinculo, result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Nenhum item liberado para vinculo foi encontrado", result = false});
                    }                    
                }
                else
                {
                    return BadRequest(new { message = "Nenhum pedido encontrado", result = false });
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
        public async Task<ActionResult<VestPedidosDTO>> getPedido(int id)
        {
            try
            {
                if (id != 0)
                {
                    var compra = await _pedidosVest.getPedido(id);

                    List<object> listaItens = new List<object>();

                    foreach (var item in compra.item)
                    {
                        var checkEstoque = await _estoque.getItemExistente(item.id, item.tamanho);
                        var checkStatus = await _status.getStatus(item.status);

                        listaItens.Add(new { 
                            item.id,
                            item.dataAlteracao,
                            item.nome,
                            item.quantidade,
                            item.status,
                            item.tamanho,
                            statusNome = checkStatus.nome,
                            estoque = checkEstoque.quantidade
                        });
                    }

                    var emp = await _usuario.GetEmp(compra.idSupervisor);
                    var status = await _status.getStatus(compra.status);

                    List<object> list = new List<object>();

                    list.Add(new {
                        Id = compra.id,
                        Nome = emp.nome,
                        Pedido = listaItens,
                        idStatus = compra.status,
                        status = status.nome,
                        idUsuario = compra.idSupervisor,
                        idUsuarioAlteracao = compra.idUsuarioAlteracao,
                        dataAlteracao = compra.dataAlteracao,
                        DataPedido = compra.dataPedido
                    });

                    return Ok(new { message = "Pedido encontrado", pedido = list, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Pedido não encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
