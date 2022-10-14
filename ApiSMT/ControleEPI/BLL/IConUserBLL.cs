using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IConUserBLL
    {
        Task<IEnumerable<DocumentoDTO>> GetDoc();
        Task<EmpContatoDTO> getEmail(int idEmpregado);
        Task<IEnumerable<EmpregadoDTO>> GetColaboradores();
        Task<EmpregadoDTO> GetEmp(int Id);
        Task<List<object>> getSuperioresColaboradores(int IdSupervisor);
        Task<SenhaDTO> Get(int id);
        Task<SenhaDTO> GetSenha(int id);
        Task<EmpContatoDTO> GetEmpCont(int id);
    }
}