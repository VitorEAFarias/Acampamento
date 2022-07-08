using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.DAL;
using System.Collections.Generic;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe que manipula os dados de colaborador
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IEmpContratosDAL _contrato;
        private readonly IConUserDAL _usuario;
        private readonly IDepartamentosDAL _departamento;
        private readonly ICargosDAL _cargo;

        /// <summary>
        /// Construtor ColaboradorController
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contrato"></param>
        /// <param name="departamento"></param>
        /// <param name="cargo"></param>
        public ColaboradorController(IConUserDAL usuario, IEmpContratosDAL contrato, IDepartamentosDAL departamento, ICargosDAL cargo)
        {
            _usuario = usuario;
            _contrato = contrato;
            _departamento = departamento;
            _cargo = cargo;
        }

        /// <summary>
        /// Lista todos os colaboradores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getListColaboradores()
        {
            try
            {
                var colaboradores = await _usuario.GetColaboradores();

                List<object> listaColaboradores = new List<object>();

                if (colaboradores != null)
                {
                    foreach (var item in colaboradores)
                    {
                        var contratoColaborador = await _contrato.getEmpContrato(item.id);

                        if (contratoColaborador != null)
                        {
                            var departamentoColaborador = await _departamento.getDepartamento(contratoColaborador.id_departamento);
                            var cargoColaborador = await _cargo.getCargo(contratoColaborador.id_cargo);

                            listaColaboradores.Add(new
                            {
                                idColaborador = item.id,
                                nome = item.nome,
                                matricula = item.matricula,
                                departamento = departamentoColaborador.titulo,
                                cargo = cargoColaborador.titulo
                            });
                        }
                        
                    }

                    return Ok(new { message = "Lista encontrada", lista = listaColaboradores, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum colaborador encontrado", result = false });   
                }                
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Seleciona um colaborador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetColaborador(int id)
        {
            try
            {
                var colaborador = await _usuario.GetEmp(id);

                if (colaborador != null)
                {
                    var contratoColaborador = await _contrato.getEmpContrato(colaborador.id);

                    if(contratoColaborador != null)
                    {
                        var departamentoColaborador = await _departamento.getDepartamento(contratoColaborador.id_departamento);
                        var cargoColaborador = await _cargo.getCargo(contratoColaborador.id_cargo);

                        var colaboradorInfo = new {
                            idColaborador = colaborador.id,
                            nome = colaborador.nome,
                            matricula = colaborador.matricula,
                            departamento = departamentoColaborador.titulo,
                            cargo = cargoColaborador.titulo
                        };

                        return Ok(new { message = "Colaborador encontrado", colaborador = colaboradorInfo, result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Contrato do colaborador não encontrado", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Colaborador não encontrado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
