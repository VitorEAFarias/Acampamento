using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vestimenta.BLL;
using Vestimenta.DTO;
using System;

namespace ApiSMT.Controllers.ControllersVestimenta
{
    /// <summary>
    /// Classe EstoqueController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueBLL _estoque;
        private readonly ILogBLL _log;

        /// <summary>
        /// Construtor EstoqueController
        /// </summary>
        /// <param name="estoque"></param>
        /// <param name="log"></param>
        public EstoqueController(IEstoqueBLL estoque, ILogBLL log)
        {
            _estoque = estoque;
            _log = log;
        }

        /// <summary>
        /// Atualiza estoque
        /// </summary>
        /// <param name="estoque"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> putEstoque(int id, [FromBody] VestEstoqueDTO estoque)
        {
            try
            {
                if (estoque != null)
                {                    
                    var checkEstoque = await _estoque.getItemEstoque(estoque.id);

                    if (checkEstoque != null)
                    {
                        estoque.dataAlteracao = DateTime.Now;

                        await _estoque.Update(estoque);

                        VestLogDTO log = new VestLogDTO();

                        log.data = DateTime.Now;
                        log.idUsuario = id;
                        log.idItem = estoque.idItem;
                        log.quantidadeAnt = checkEstoque.quantidade;
                        log.quantidadeDep = checkEstoque.quantidade + estoque.quantidade;
                        log.tamanho = checkEstoque.tamanho;

                        var insereLog = await _log.Insert(log);

                        if (insereLog != null)
                        {
                            return Ok(new { message = "Quantidade em estoque atualizada com sucesso!!!", result = true });
                        }
                        else
                        {
                            return Ok(new { message = "Quantidade em Estoque atualizada com sucesso!!!, ERRO ao inserir LOG", result = true });
                        }                        
                    }
                    else
                    {
                        return BadRequest(new { message = "Item não encontrado em estoque", result = false });   
                    }                    
                }
                else
                {
                    return BadRequest(new { message = "Nenhuma informação enviada!!!", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todo o estoque
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> getEstoque()
        {
            try
            {
                var estoque = await _estoque.getEstoque();

                return Ok(new { message = "lista encontrada", result = true, lista = estoque });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona um item do estoque
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<VestEstoqueDTO>> getItemStoque(int id)
        {
            try
            {
                if (id != 0)
                {
                    var estoque = await _estoque.getItemEstoque(id);

                    return Ok(new { message = "Item do estoque encontrado", estoque = estoque, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Item do estoque não encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
