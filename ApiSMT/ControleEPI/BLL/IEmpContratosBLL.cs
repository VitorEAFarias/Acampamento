using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IEmpContratosBLL
    {
        Task<IEnumerable<EmpContratosDTO>> getContratos();
        Task<EmpContratosDTO> getContrato(int Id);
        Task<EmpContratosDTO> getEmpContrato(int Id);
    }
}
