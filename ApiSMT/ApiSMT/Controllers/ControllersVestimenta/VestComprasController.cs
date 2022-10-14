using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vestimenta.BLL;
using Vestimenta.DTO;
using System;
using System.Collections.Generic;
using ControleEPI.BLL;

namespace ApiSMT.Controllers.ControllersVestimenta
{
    /// <summary>
    /// Classe de Compras
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VestComprasController : ControllerBase
    {
        private readonly IComprasVestBLL _comprasVest;
        private readonly IEstoqueBLL _estoque;
        private readonly IVestimentaBLL _vestimenta;
        private readonly ILogBLL _log;
        private readonly IPedidosVestBLL _pedidos;
        private readonly IVestRepositorioBLL _repositorio;
        private readonly IConUserBLL _conuser;
        private readonly IStatusVestBLL _status;

        /// <summary>
        /// Construtor de Compras
        /// </summary>
        /// <param name="comprasVest"></param>
        /// <param name="estoque"></param>
        /// <param name="vestimenta"></param>
        /// <param name="log"></param>
        /// <param name="pedidos"></param>
        /// <param name="repositorio"></param>
        /// <param name="conuser"></param>
        /// <param name="status"></param>
        public VestComprasController(IComprasVestBLL comprasVest, IEstoqueBLL estoque, IVestimentaBLL vestimenta, ILogBLL log, IPedidosVestBLL pedidos, 
            IVestRepositorioBLL repositorio, IConUserBLL conuser, IStatusVestBLL status)
        {
            _comprasVest = comprasVest;
            _estoque = estoque;
            _vestimenta = vestimenta;
            _log = log;
            _pedidos = pedidos;
            _repositorio = repositorio;
            _conuser = conuser;
            _status = status;
        }

        /// <summary>
        /// Cadastra uma nova compra
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<VestComprasDTO>> postCadastrarCompra([FromBody] VestComprasDTO compra)
        {
            try
            {
                if (compra != null)
                {
                    var novaCompra = await _comprasVest.Insert(compra);

                    if (novaCompra != null)
                    {
                        return Ok(new { message = "Compra cadastrada com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Erro ao cadastrar nova compra", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Erro ao cadastrar compra " + compra, result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Efetuar compra dos itens
        /// </summary>
        /// <param name="comprar"></param>
        /// <returns></returns>
        [HttpPut("comprar/{idCompra}")]
        public async Task<ActionResult> comprar([FromBody] VestComprasDTO comprar)
        {
            try
            {
                var checkCompra = await _comprasVest.getCompra(comprar.id);

                string mensagem = string.Empty;

                if (checkCompra != null)
                {
                    foreach (var repositorioItens in checkCompra.itensRepositorio)
                    {
                        List<object> itensAvulso = new List<object>();
                        VestLogDTO log = new VestLogDTO();

                        foreach (var idRepositorio in repositorioItens.idRepositorio)
                        {
                            var getRepositorio = await _repositorio.getRepositorio(idRepositorio);
                            List<ItemDTO> getItemPedido = new List<ItemDTO>();                            

                            if (getRepositorio != null)
                            {
                                VestPedidosDTO getPedido = await _pedidos.getPedido(getRepositorio.idPedido);                                

                                foreach (var itens in getPedido.item)
                                {
                                    if (itens.id == repositorioItens.idItem && itens.tamanho == repositorioItens.tamanho)
                                    {
                                        getItemPedido.Add(new ItemDTO
                                        {
                                            id = itens.id,
                                            nome = itens.nome,
                                            tamanho = itens.tamanho,
                                            quantidade = itens.quantidade,
                                            status = 7,
                                            dataAlteracao = DateTime.Now
                                        });
                                    }
                                    else
                                    {
                                        getItemPedido.Add(new ItemDTO
                                        {
                                            id = itens.id,
                                            nome = itens.nome,
                                            tamanho = itens.tamanho,
                                            quantidade = itens.quantidade,
                                            status = itens.status,
                                            dataAlteracao = itens.dataAlteracao
                                        });
                                    }
                                }

                                int contador = 0;

                                foreach (var status in getItemPedido)
                                {
                                    if (status.status == 2 || status.status == 7 || status.status == 3)
                                        contador++;
                                }

                                getPedido.item = getItemPedido;

                                if (contador == getItemPedido.Count)
                                {
                                    getPedido.status = 2;
                                }
                                else
                                {
                                    getPedido.status = getPedido.status;
                                }

                                await _pedidos.Update(getPedido);
                            }
                            else
                            {
                                itensAvulso.Add(new 
                                {
                                    id = repositorioItens.idItem,
                                    tamanho = repositorioItens.tamanho,
                                    preco = repositorioItens.preco,
                                    quantidade = repositorioItens.quantidade
                                });
                            }
                        }

                        if (itensAvulso == null)
                        {
                            VestEstoqueDTO getEstoque = await _estoque.getItemExistente(repositorioItens.idItem, repositorioItens.tamanho);

                            var quantidadeAnterior = getEstoque.quantidade;

                            if (getEstoque != null)
                            {
                                getEstoque.quantidade = getEstoque.quantidade + repositorioItens.quantidade;
                                getEstoque.dataAlteracao = DateTime.Now;

                                await _estoque.Update(getEstoque);                                

                                log.data = DateTime.Now;
                                log.idUsuario = comprar.idUsuario;
                                log.idItem = repositorioItens.idItem;
                                log.quantidadeAnt = quantidadeAnterior;
                                log.quantidadeDep = getEstoque.quantidade;
                                log.tamanho = repositorioItens.tamanho;

                                await _comprasVest.Update(comprar);

                                var insereLogEstoqueDisponivel = await _log.Insert(log);

                                if (insereLogEstoqueDisponivel != null)
                                {
                                    mensagem = "Compra realizada com sucesso!!!";
                                }
                                else
                                {
                                    mensagem = "Erro ao realizar compra!!!";
                                }
                            }
                            else
                            {
                                var checkItem = await _vestimenta.getVestimenta(repositorioItens.idItem);

                                if (checkItem != null)
                                {
                                    VestEstoqueDTO novoItem = new VestEstoqueDTO();

                                    novoItem.idItem = repositorioItens.idItem;
                                    novoItem.quantidade = repositorioItens.quantidade;
                                    novoItem.tamanho = repositorioItens.tamanho;
                                    novoItem.dataAlteracao = DateTime.Now;
                                    novoItem.quantidadeUsado = 0;

                                    var insereEstoque = await _estoque.Insert(novoItem);

                                    if (insereEstoque != null)
                                    {
                                        VestLogDTO logEstoqueIndisponivel = new VestLogDTO();

                                        logEstoqueIndisponivel.data = DateTime.Now;
                                        logEstoqueIndisponivel.idUsuario = comprar.idUsuario;
                                        logEstoqueIndisponivel.idItem = repositorioItens.idItem;
                                        logEstoqueIndisponivel.quantidadeAnt = getEstoque.quantidade;
                                        logEstoqueIndisponivel.quantidadeDep = novoItem.quantidade;
                                        logEstoqueIndisponivel.tamanho = repositorioItens.tamanho;

                                        comprar.status = 7;

                                        await _comprasVest.Update(comprar);

                                        var insereLogEstoqueIndisponivel = await _log.Insert(logEstoqueIndisponivel);

                                        if (insereLogEstoqueIndisponivel != null)
                                        {
                                            mensagem = "Compra efetuada com sucesso!!!";
                                        }
                                        else
                                        {
                                            mensagem = "Erro ao efetuar compra!!!";
                                        }
                                    }
                                    else
                                    {
                                        mensagem = "Vestimenta não encontrara";
                                    }
                                }
                                else
                                {
                                    mensagem = "Erro ao inserir item no estoque!!!";
                                }
                            }
                        }
                        else
                        {
                            var getEstoque = await _estoque.getItemExistente(repositorioItens.idItem, repositorioItens.tamanho);

                            var quantidadeAnt = getEstoque.quantidade; 

                            getEstoque.quantidade = getEstoque.quantidade + repositorioItens.quantidade;
                            getEstoque.dataAlteracao = DateTime.Now;

                            var getCompra = await _comprasVest.getCompra(comprar.id);

                            if (getCompra != null)
                            {
                                getCompra.dataCompra = DateTime.Now;
                                getCompra.status = 2;
                                getCompra.descricao = comprar.descricao;

                                await _comprasVest.Update(getCompra);
                                await _estoque.Update(getEstoque);

                                log.data = DateTime.Now;
                                log.idUsuario = comprar.idUsuario;
                                log.idItem = repositorioItens.idItem;
                                log.quantidadeAnt = quantidadeAnt;
                                log.quantidadeDep = getEstoque.quantidade;
                                log.tamanho = repositorioItens.tamanho;

                                var inserLogAvulso = await _log.Insert(log);

                                if (inserLogAvulso != null)
                                {
                                    mensagem = "Compra avulsa realizada com sucesso!!!";
                                }
                            }                            
                        }                        
                    }

                    return Ok(new { message = mensagem, result = true });
                }
                else 
                {
                    return BadRequest(new { message = "Nenhuma compra encontrada", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                
            }
        }        

        /// <summary>
        /// Lista todas as compras
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> getCompras()
        {
            try
            {                                
                var compras = await _comprasVest.getCompras();
                List<object> lista = new List<object>();

                foreach (var item in compras)
                {
                    var getUsuario = await _conuser.GetEmp(item.idUsuario);
                    var getStatus = await _status.getStatus(item.status);

                    lista.Add(new
                    {
                        id = item.id,
                        nome = getUsuario.nome,
                        status = getStatus.nome,
                        item.dataCompra,
                        item.itensRepositorio
                    });
                                        
                }

                return Ok(new { message = "lista encontrada", result = true, lista = lista });
            }
            catch (Exception ex)
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
        public async Task<ActionResult<VestComprasDTO>> getCompra(int id)
        {
            try
            {
                var checkCompra = await _comprasVest.getCompra(id);

                if (checkCompra != null)
                {
                    var getUsuario = await _conuser.GetEmp(checkCompra.idUsuario);
                    var getStatus = await _status.getStatus(checkCompra.status);

                    List<object> lista = new List<object>();

                    foreach (var item in checkCompra.itensRepositorio)
                    {
                        var checkVestimenta = await _vestimenta.getVestimenta(item.idItem);

                        lista.Add(new {
                            item.idItem,
                            item.idRepositorio,
                            item.tamanho,
                            item.quantidade,
                            item.preco,
                            nome = checkVestimenta.nome
                        });
                    }

                    var itens = new
                    {
                        idUsuario = getUsuario.id,
                        id = checkCompra.id,
                        nome = getUsuario.nome,
                        idStatus = getStatus.id,
                        status = getStatus.nome,
                        checkCompra.dataCompra,
                        itensRepositorio = lista
                    };

                    return Ok(new { message = "Compra encontrada", compra = itens, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Compra não encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
