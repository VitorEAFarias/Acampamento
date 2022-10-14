using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface ICategoriasBLL
    {
        Task<IEnumerable<CategoriaDTO>> getCategorias();
        Task<CategoriaDTO> getCategoria(int Id);
        Task<CategoriaDTO> getNomeCategoria(string nome);
        Task<CategoriaDTO> Insert(CategoriaDTO categoria);
        Task Update(CategoriaDTO categoria);
        Task Delete(int Id);
    }
}
