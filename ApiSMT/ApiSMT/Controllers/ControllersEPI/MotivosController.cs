using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.BLL;
using ControleEPI.DTO;
using System;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe de manipulação de motivos de pedidos de epi's
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MotivosController : ControllerBase
    {
        private readonly IMotivosBLL _motivo;

        /// <summary>
        /// Construtor MotivosController
        /// </summary>
        /// <param name="motivo"></param>
        public MotivosController(IMotivosBLL motivo)
        {
            _motivo = motivo;
        }

        /// <summary>
        /// Lista todos os motivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> getMotivos()
        {
            try
            {
                var motivos = await _motivo.getMotivos();

                return Ok(new { message = "lista encontrada", result = true, lista = motivos });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona um motivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id")]
        public async Task<ActionResult<MotivoDTO>> getMotivo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var motivo = await _motivo.getMotivo(id);

                    return Ok(new { message = "Motivo encontrado", motivo = motivo.nome, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Motivo não encontrado", result = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
