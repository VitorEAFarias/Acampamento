using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface ICargosBLL
    {
        Task<IEnumerable<CargosDTO>> getCargos();
        Task<CargosDTO> getCargo(int Id);
    }
}
