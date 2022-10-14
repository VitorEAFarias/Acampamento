using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vestimenta.BLL;
using Vestimenta.DTO;
using System;
using ControleEPI.BLL;
using System.Collections.Generic;
using ApiSMT.Utilitários;

namespace ApiSMT.Controllers.ControllersVestimenta
{
    /// <summary>
    /// Classe VestVinculoController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VestVinculoController : ControllerBase
    {
        private readonly IVestVinculoBLL _vinculo;
        private readonly IConUserBLL _usuario;
        private readonly IEstoqueBLL _estoque;
        private readonly IVestimentaBLL _vestimenta;
        private readonly ILogBLL _log;
        private readonly IVestItemVinculoBLL _itemVinculo;
        private readonly IPedidosVestBLL _pedidos;

        /// <summary>
        /// Construtor VestVinculoController
        /// </summary>
        /// <param name="vinculo"></param>
        /// <param name="usuario"></param>
        /// <param name="estoque"></param>
        /// <param name="vestimenta"></param>
        /// <param name="log"></param>
        /// <param name="itemVinculo"></param>
        /// <param name="pedidos"></param>
        public VestVinculoController(IVestVinculoBLL vinculo, IConUserBLL usuario, IEstoqueBLL estoque, IVestimentaBLL vestimenta, ILogBLL log, 
            IVestItemVinculoBLL itemVinculo, IPedidosVestBLL pedidos)
        {
            _vinculo = vinculo;
            _usuario = usuario;
            _estoque = estoque;
            _vestimenta = vestimenta;
            _log = log;
            _itemVinculo = itemVinculo;
            _pedidos = pedidos;
        }

        /// <summary>
        /// Insere um novo status
        /// </summary>
        /// <param name="itemVinculo"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idUsuarioVinculo"></param>
        /// <returns></returns>
        [HttpPut("{idUsuario}/{idUsuarioVinculo}")]
        public async Task<ActionResult> postVinculo(int idUsuario, int idUsuarioVinculo, [FromBody] VestItemVinculoDTO itemVinculo)
        {
            try
            {
                var checkItemVinculo = await _itemVinculo.getItemVinculo(itemVinculo.id);

                if (checkItemVinculo != null)
                {
                    var usuario = await _usuario.GetEmp(idUsuario);

                    if (usuario != null)
                    {
                        var supervisor = await _usuario.GetEmp(idUsuarioVinculo);

                        if (supervisor != null)
                        {
                            var checkEstoque = await _estoque.getItemExistente(itemVinculo.idItem, itemVinculo.tamanho);
                            var nomeVest = await _vestimenta.getVestimenta(itemVinculo.idItem);

                            for (int i=0; i<itemVinculo.quantidade; i++)
                            {
                                VestVinculoDTO vincular = new VestVinculoDTO();

                                vincular.idUsuario = usuario.id;
                                vincular.idUsuarioVinculo = supervisor.id;
                                vincular.idVestimenta = itemVinculo.idItem;
                                vincular.dataVinculo = DateTime.Now;
                                vincular.status = 1;
                                vincular.tamanhoVestVinculo = itemVinculo.tamanho;

                                var insereVinculo = await _vinculo.Insert(vincular);                                
                            }

                            var checkitemVinculo = await _itemVinculo.getItemVinculo(itemVinculo.id);

                            checkitemVinculo.quantidade = checkitemVinculo.quantidade - itemVinculo.quantidade;

                            if (checkitemVinculo.quantidade == 0)
                            {
                                checkitemVinculo.status = 6;
                            }

                            checkitemVinculo.dataAlteracao = DateTime.Now;

                            await _itemVinculo.Update(checkitemVinculo);

                            VestLogDTO log = new VestLogDTO();

                            log.data = DateTime.Now;
                            log.idUsuario = usuario.id;
                            log.idItem = nomeVest.id;
                            log.quantidadeAnt = checkEstoque.quantidade;
                            log.quantidadeDep = checkEstoque.quantidade - itemVinculo.quantidade;

                            await _log.Insert(log);

                            return Ok(new { message = "Vinculo inserido com sucesso!!!", result = true });                                                        
                        }
                        else
                        {
                            return BadRequest(new { message = "Supervisor não encontrado", result = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Colaborador não encontrado", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { messsage = "Nenhum vinculo selecionado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Função que vincula itens com colaborador
        /// </summary>
        /// <param name="itens"></param>
        /// <param name="idUsuario"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        [HttpPut("verificacao/{idUsuario}/{senha}")]
        public async Task<ActionResult> aceitaVinculo(int idUsuario, string senha, [FromBody] List<int> itens)
        {
            try
            {
                if (itens != null)
                {
                    var checkUsuario = await _usuario.GetSenha(idUsuario);

                    if (checkUsuario != null)
                    {
                        GerarMD5 md5 = new GerarMD5();

                        var senhaMD5 = md5.GeraMD5(senha);

                        if (checkUsuario.senha == senhaMD5)
                        {
                            foreach (var item in itens)
                            {
                                var checkItem = await _vinculo.getVinculo(item);

                                if (checkItem != null)
                                {
                                    checkItem.status = 6;

                                    await _vinculo.Update(checkItem);
                                }
                            }

                            return Ok(new { message = "Itens vinculados com sucesso!!!", result = true });
                        }
                        else
                        {
                            return BadRequest(new { message = "Senha incorreta", result = false });
                        }                        
                    }
                    else
                    {
                        return BadRequest(new { message = "Nenhum usuario encontrado!!!", result = false });
                    }         
                }
                else
                {
                    return BadRequest(new { message = "Nenhum item enviado para vincular", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get de vinculos pendentes
        /// </summary>
        /// <param name="idSatus"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("status/{idSatus}/{idUsuario}")]
        public async Task<ActionResult> getStatus(int idSatus, int idUsuario)
        {
            try
            {
                var itensPendentes = await _vinculo.getVinculoPendente(idSatus, idUsuario);

                if (itensPendentes != null)
                {
                    List<object> vinculoPendente = new List<object>();

                    foreach (var item in itensPendentes)
                    {
                        var checkVestimenta = await _vestimenta.getVestimenta(item.idVestimenta);
                        var checkUsuario = await _usuario.GetEmp(item.idUsuario);
                        var checkSupervisor = await _usuario.GetEmp(item.idUsuarioVinculo);

                        vinculoPendente.Add(new {
                            id = item.id,
                            idItem = item.idVestimenta,
                            nomeUsuario = checkUsuario.nome,
                            nomeSupervisor = checkSupervisor.nome,
                            nomeVestimenta = checkVestimenta.nome,
                            tamanho = item.tamanhoVestVinculo,
                            data = item.dataVinculo
                        });
                    }

                    return Ok(new { message = "Itens pendentes encontrados!!!", result = true, lista = vinculoPendente });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum colaborador encontrado!!!", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get de colaboradores e seu supervisor
        /// </summary>
        /// <param name="idSupervisor"></param>
        /// <returns></returns>
        [HttpGet("supervisor/{idSupervisor}")]
        public async Task<ActionResult> getSuperioresColaboradores(int idSupervisor)
        {
            try
            {
                var checkSupervisor = await _usuario.getSuperioresColaboradores(idSupervisor);

                if (checkSupervisor != null)
                {
                    return Ok(new { message = "Colaboradores encontrados!!!", result = true, lista = checkSupervisor });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum colaborador encontrado!!!", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os vinculos de vestimenta
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> getVinculoVestimentas()
        {
            try
            {
                var vinculosVest = await _vinculo.getVinculos();

                return Ok(new { message = "lista encontrada", result = true, lista = vinculosVest });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona uma vinculo com vestimentas
        /// </summary>
        /// <param name="idPedido"></param>
        /// <returns></returns>
        [HttpGet("{idPedido}")]
        public async Task<ActionResult<VestimentaDTO>> getVinculoVestimenta(int idPedido)
        {
            try
            {
                var vinculoVest = await _itemVinculo.getItensPedido(idPedido);

                if (vinculoVest != null)
                {
                    return Ok(new { message = "Itens encontrados", itens = vinculoVest, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Vinculos não encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// get todas as situações do colaborador
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("situacoes/{idUsuario}")]
        public async Task<ActionResult> getSituações(int idUsuario)
        {
            try
            {
                var getPendente = await _vinculo.getItensUsuarios(idUsuario);
                var getPedidos = await _pedidos.getPedidosUsuarios(idUsuario);

                List<object> pendentes = new List<object>();
                List<object> vinculados = new List<object>();
                List<object> pedidosFinalizados = new List<object>();
                List<object> pedidosPendentes = new List<object>();                
                List<object> pedidosReprovados = new List<object>();

                foreach (var item in getPendente)
                {
                    if (item.status == 6)
                    {
                        vinculados.Add(new { 
                            item = item.id
                        });
                    }
                    else
                    {
                        pendentes.Add(new {
                            item = item.id
                        });
                    }
                }

                foreach (var item in getPedidos)
                {
                    if (item.status == 2)
                    {
                        pedidosFinalizados.Add(new { 
                            item = item.id
                        });

                    }
                    else if (item.status == 1)
                    {
                        pedidosPendentes.Add(new {
                            item = item.id
                        });
                    }
                    else
                    {
                        pedidosReprovados.Add(new {
                            item = item.id
                        });
                    }
                }

                return Ok(new { message = "Numeros encontrados!!!", result = true, vinculado = vinculados.Count, pendente = pendentes.Count, pedidosFinalizados = pedidosFinalizados.Count,
                    pedidosPendentes = pedidosPendentes.Count, pedidosReprovados = pedidosReprovados.Count });
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Pega todos os itens vinculados com um usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("vinculados/{idUsuario}")]
        public async Task<ActionResult<VestVinculoDTO>> getItensVinculados(int idUsuario)
        {
            try
            {
                var checkVinculados = await _vinculo.getItensVinculados(idUsuario);

                if (checkVinculados != null)
                {
                    List<object> lista = new List<object>();

                    foreach (var item in checkVinculados)
                    {
                        var vestimenta = await _vestimenta.getVestimenta(item.idVestimenta);

                        lista.Add(new { 
                            idItem = vestimenta.id,
                            idVinculado = item.id,
                            Vestimenta = vestimenta.nome,
                            Tamanho = item.tamanhoVestVinculo,
                            DataVinculo = item.dataVinculo,
                            Status = item.status
                        });
                    }

                    if (lista != null)
                    {
                        return Ok(new { message = "Itens encontrados", lista = lista, result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Nenhum item pendente encontrado", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Nenhum item vinculado com esse colaborador", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
