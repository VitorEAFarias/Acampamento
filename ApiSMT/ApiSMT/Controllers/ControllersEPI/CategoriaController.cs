using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO;
using ControleEPI.BLL;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe de manipulação de categorias
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriasBLL _categorias;
        private readonly IProdutosBLL _produtos;
        private readonly IConUserBLL _usuario;

        /// <summary>
        /// Construtor CategoriaController
        /// </summary>
        /// <param name="categorias"></param>
        /// <param name="produtos"></param>
        /// <param name="usuario"></param>
        public CategoriaController(ICategoriasBLL categorias, IProdutosBLL produtos, IConUserBLL usuario)
        {
            _categorias = categorias;
            _produtos = produtos;
            _usuario = usuario;
        }

        /// <summary>
        /// Lista todas as categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> getCategorias()
        {
            try
            {
                var categorias = await _categorias.getCategorias();

                if (categorias != null)
                {
                    return Ok(new { message = "Lista encontrada", lista = categorias, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Nenhuma categoria encontrado", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Seleciona uma categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            try
            {
                var categoria = await _categorias.getCategoria(id);

                if (categoria != null)
                {
                    return Ok(new { message = "Categoria '"+categoria.nome+"' encontrada", categoria = categoria, result = true });
                }
                else
                {
                    return BadRequest(new { message = "Categoria não encontrada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Insere uma nova categoria
        /// </summary>
        /// <param name="categoria"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<CategoriaDTO>> PostCategoria([FromRoute]int id, [FromBody] CategoriaDTO categoria)
        {
            try
            {
                var usuario = await _usuario.GetEmp(id);

                if (usuario != null)
                {
                    if (categoria != null)
                    {
                        var checkCategoria = await _categorias.getNomeCategoria(categoria.nome);

                        if (checkCategoria == null)
                        {
                            var novaCategoria = await _categorias.Insert(categoria);

                            return Ok(new { message = "Categoria inserida com sucesso!!!", result = true, data = novaCategoria });
                        }
                        else
                        {
                            return BadRequest(new { message = "Ja existe uma categoria com esse nome", result = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Pro favor preencha todos os campos", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Usuario não encontrado", result = false});
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);                
            }            
        }

        /// <summary>
        /// Deleta uma categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var deletaCategoria = await _categorias.getCategoria(id);

                if (deletaCategoria != null)
                {
                    var categoriaProduto = await _produtos.getCategoriaProduto(deletaCategoria.id);

                    if (categoriaProduto == null)
                    {
                        await _categorias.Delete(deletaCategoria.id);

                        return Ok(new { message = "Categoria excluida com sucesso!!!", result = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Existem produtos vinculados a essa categoria", result = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Nenhuma categoria encontrada", result = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        /// <summary>
        /// Atualiza uma categoria
        /// </summary>
        /// <param name="categoria"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategoria([FromRoute] int id, [FromBody] CategoriaDTO categoria)
        {
            try
            {
                var usuario = await _usuario.GetEmp(id);

                if (usuario != null)
                {
                    if (categoria.id != 0)
                    {
                        await _categorias.Update(categoria);

                        return Ok(new { message = "Categoria '" + categoria.nome + "' atualizada com sucesso!!!", data = true });
                    }
                    else
                    {
                        return BadRequest(new { message = "Nenhuma categoria selecionada", data = false });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Usuário não encontrado", data = false });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
    }
}
