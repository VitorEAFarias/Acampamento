using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IDepartamentosBLL
    {
        Task<IEnumerable<DepartamentosDTO>> getDepartamentos();
        Task<DepartamentosDTO> getDepartamento(int Id);
    }
}
