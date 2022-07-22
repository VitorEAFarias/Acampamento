using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IConUserDAL
    {
        Task<IEnumerable<DocumentoDTO>> GetDoc();
        Task<IEnumerable<EmpregadoDTO>> GetColaboradores();
        Task<EmpregadoDTO> GetEmp(int Id);        
        Task<SenhaDTO> Get(int id);
        Task<SenhaDTO> GetSenha(int id);
        Task<EmpContatoDTO> GetEmpCont(int id);
    }
}