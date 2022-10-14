using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.BLL;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe de manipulação de fornecedores
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedoresBLL _fornecedor;
        private readonly IProdutosBLL _produtos;
        private readonly IConUserBLL _usuario;

        /// <summary>
        /// Construtor FornecedorController
        /// </summary>
        /// <param name="fornecedor"></param>
        /// <param name="produtos"></param>
        /// <param name="usuario"></param>
        public FornecedorController(IFornecedoresBLL fornecedor, IProdutosBLL produtos, IConUserBLL usuario)
        {
            _fornecedor = fornecedor;
            _produtos = produtos;
            _usuario = usuario;
        }

        /// <summary>
        /// Lista de fornecedores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getFornecedores()
        {
            try
            {
                var fornecedores = await _fornecedor.getFornecedores();

                if (fornecedores != null)
                {
                    return Ok(new { message = "Fornecedores encontrados", lista = fornecedores, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhum fornecedor encontrado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Seleciona um fornecedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorDTO>> getFornecedor(int id)
        {
            try
            {
                var fornecedor = await _fornecedor.getFornecedor(id);

                if (fornecedor != null)
                {
                    return Ok(new { message = "Fornecedor '" + fornecedor.nome + "' encontrado", fornecedor = fornecedor, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Fornecedor não encontrado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Insere um novo fornecedor
        /// </summary>
        /// <param name="fornecedor"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<FornecedorDTO>> PostFornecedor([FromRoute]int id, [FromBody] FornecedorDTO fornecedor)
        {
            try
            {
                var usuario = await _usuario.GetEmp(id);

                if (usuario != null)
                {
                    if (fornecedor != null)
                    {
                        var checkFornecedor = await _fornecedor.getNomeFornecedor(fornecedor.nome);
                        if (checkFornecedor != null)
                        {
                            return BadRequest(new { message = "Ja existe um fornecedor chamado: " + fornecedor.nome, result = false });
                        }
                        else
                        {
                            var novoFornecedor = await _fornecedor.Insert(fornecedor);
                            return Ok(new { message = "Fornecedor inserido com sucesso!!!", result = true, data = novoFornecedor });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Erro ao inserir fornecedor " + fornecedor.nome, result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Usuario não encontrado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um fornecedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var deletaFornecedor = await _fornecedor.getFornecedor(id);

                if (deletaFornecedor != null)
                {
                    var fornecedorProduto = await _produtos.getFornecedorProduto(deletaFornecedor.id);
                    if (fornecedorProduto == null)
                    {
                        await _fornecedor.Delete(deletaFornecedor.id);
                        return Ok(new { message = deletaFornecedor.nome + " Excluido com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Existem fornecedores vinculados a produtos", result = false });   
                    }                    
                }
                else
                {
                    return BadRequest (new { message = "Fornecedor não encontrado", result = false });
                }
                    
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados de um fornecedor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fornecedor"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutFornecedor([FromRoute] int id, [FromBody] FornecedorDTO fornecedor)
        {
            try
            {
                var usuario = await _usuario.GetEmp(id);

                if (usuario != null)
                {
                    if (fornecedor.id != 0)
                    {
                        await _fornecedor.Update(fornecedor);

                        return Ok(new { message = fornecedor.nome + " Atualizado com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Nenhum fornecedor encontrado!!!", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Usuário não encontrado", result = false});
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
