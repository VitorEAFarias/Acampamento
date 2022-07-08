using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IDepartamentosDAL
    {
        Task<IEnumerable<DepartamentosDTO>> getDepartamentos();
        Task<DepartamentosDTO> getDepartamento(int Id);
    }
}
