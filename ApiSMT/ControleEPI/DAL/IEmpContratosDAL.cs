using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IEmpContratosDAL
    {
        Task<IEnumerable<EmpContratosDTO>> getContratos();
        Task<EmpContratosDTO> getContrato(int Id);
        Task<EmpContratosDTO> getEmpContrato(int Id);
    }
}
