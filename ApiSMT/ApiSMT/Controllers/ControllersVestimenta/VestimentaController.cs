using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vestimenta.BLL;
using Vestimenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiSMT.Controllers.ControllersVestimenta
{
    /// <summary>
    /// Classe de vestimentas
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VestimentaController : ControllerBase
    {
        private readonly IVestimentaBLL _vestimenta;
        private readonly IEstoqueBLL _estoque;

        /// <summary>
        /// Construtor VestimentaController
        /// </summary>
        /// <param name="vestimenta"></param>
        /// <param name="estoque"></param>
        public VestimentaController(IVestimentaBLL vestimenta, IEstoqueBLL estoque)
        {
            _vestimenta = vestimenta;
            _estoque = estoque;
        }

        /// <summary>
        /// Insere uma nova vestimenta
        /// </summary>
        /// <param name="vestimenta"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<VestimentaDTO>> postVestimenta([FromForm] VestimentaDTO vestimenta)
        {
            try
            {
                if (vestimenta != null)
                {
                    var checkVestimenta = await _vestimenta.getNomeVestimenta(vestimenta.nome);

                    if (checkVestimenta != null)
                    {
                        return BadRequest(new { message = "Ja existe uma vestimenta chamada: " + vestimenta.nome, result = false });
                    }
                    else
                    {
                        VestimentaDTO inserirVestimenta = new VestimentaDTO();

                        inserirVestimenta.ativo = 1;
                        inserirVestimenta.foto = vestimenta.foto;
                        inserirVestimenta.preco = vestimenta.preco;
                        inserirVestimenta.dataCadastro = DateTime.Now;
                        inserirVestimenta.tamanho = vestimenta.tamanho;
                        inserirVestimenta.nome = vestimenta.nome;

                        var novaVestimenta = await _vestimenta.Insert(inserirVestimenta);

                        if (novaVestimenta != null)
                        {
                            foreach (var tamanho in novaVestimenta.tamanho)
                            {
                                VestEstoqueDTO estoque = new VestEstoqueDTO();

                                estoque.idItem = novaVestimenta.id;
                                estoque.quantidade = 0;
                                estoque.quantidadeUsado = 0;
                                estoque.dataAlteracao = DateTime.Now;
                                estoque.tamanho = tamanho.tamanho;

                                var attEstoque = await _estoque.Insert(estoque);
                            }

                            return Ok(new { message = "Vestimenta inserido com sucesso!!!", result = true, data = novaVestimenta });
                        }
                        else
                        {
                            return BadRequest(new { message = "Erro ao inserir vestimenta!!!", result = false });
                        }
                    }
                }
                else
                {
                    return BadRequest(new { message = "Erro ao inserir vestimenta " + vestimenta.nome, result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza Vestimenta
        /// </summary>
        /// <param name="vestimenta"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> putVestimenta([FromForm] VestimentaDTO vestimenta)
        {
            try
            {
                VestimentaDTO checkVestimenta = await _vestimenta.getVestimenta(vestimenta.id);

                if (checkVestimenta != null)
                {
                    var atualizaVestimenta = await _vestimenta.Update(vestimenta);

                    if (atualizaVestimenta != null)
                    {
                        IEnumerable<Tamanho> listaTotal = vestimenta.tamanho.Except(checkVestimenta.tamanho);
                        VestEstoqueDTO insereEstoque = new VestEstoqueDTO();

                        foreach (var tamanho in listaTotal)
                        {
                            var estoque = await _estoque.getItemExistente(checkVestimenta.id, tamanho.tamanho);

                            if (estoque == null)
                            {
                                insereEstoque.idItem = vestimenta.id;
                                insereEstoque.quantidade = 0;
                                insereEstoque.tamanho = tamanho.tamanho;
                                insereEstoque.dataAlteracao = DateTime.Now;
                                insereEstoque.quantidadeUsado = 0;

                                insereEstoque = await _estoque.Insert(insereEstoque);
                            }
                        }

                        if (string.IsNullOrEmpty(Convert.ToString(insereEstoque)))
                        {
                            return Ok(new { message = "Vestimentas e tamanhos atualizados com sucesso!!!", result = true });
                        }
                        else
                        {
                            return Ok(new { message = "Vestimenta atualizada com sucesso!!!", result = true });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Erro ao atualizar a Vestimenta!!!", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Nenhum item encontrado!!!", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Desativa vestimenta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{status}")]
        public async Task<ActionResult> desativaVestimenta(int id, int status)
        {
            try
            {
                var desativaVes = await _vestimenta.getVestimenta(id);                

                if (desativaVes != null)
                {
                    desativaVes.ativo = status;                        
                    
                    await _vestimenta.Update(desativaVes);

                    return Ok(new { message = desativaVes.nome + " Desativado com sucesso!!!", result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhuma vestimenta encontrada!!!", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos as vestimentas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> getVestimentas()
        {
            try
            {
                var vestimenta = await _vestimenta.getVestimentas();
                                               
                List<object> tamanhoTotal = new List<object>();                

                if (vestimenta != null)
                {
                    foreach (var item in vestimenta)
                    {
                        var quantidadeEstoque = await _estoque.getItensExistentes(item.id);

                        List<object> tamanhosRam = new List<object>();

                        tamanhosRam.Add(new
                        {
                            nome = item.nome,
                            idVestimenta = item.id,
                            tamanho = item.tamanho,
                            quantidade = quantidadeEstoque,
                            preco = item.preco,
                            Foto = item.foto,
                            Ativo = item.ativo
                        });                         

                        tamanhoTotal.AddRange(tamanhosRam);                        
                    }

                    return Ok(new { message = "lista encontrada", result = true, lista = tamanhoTotal });
                }
                else
                {
                    return BadRequest(new { message = "Nenhuma vestimenta encontrada!!!", result = false });
                }                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona uma vestimenta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<VestimentaDTO>> getVestimenta(int id)
        {
            try
            {
                if (id != 0)
                {
                    var vestimenta = await _vestimenta.getVestimenta(id);
                    var quantidadeEstoque = await _estoque.getItensExistentes(vestimenta.id);

                    var tamanhosRam  = new
                    {
                        nome = vestimenta.nome,
                        idVestimenta = vestimenta.id,
                        tamanho = vestimenta.tamanho,
                        quantidade = quantidadeEstoque,
                        preco = vestimenta.preco,
                        Foto = vestimenta.foto,
                        Ativo = vestimenta.ativo
                    };

                    return Ok(new { message = "Vestimenta encontrada", vestimenta = tamanhosRam, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Vestimenta não encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
