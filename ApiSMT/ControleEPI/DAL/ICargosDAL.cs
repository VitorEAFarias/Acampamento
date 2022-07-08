using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface ICargosDAL
    {
        Task<IEnumerable<CargosDTO>> getCargos();
        Task<CargosDTO> getCargo(int Id);
    }
}
