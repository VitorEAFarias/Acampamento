using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface ILogEstoqueBLL
    {
        Task<LogEstoqueDTO> Insert(LogEstoqueDTO logEstoque);
        Task<LogEstoqueDTO> GetLogEstoque(int Id);
        Task<IEnumerable<LogEstoqueDTO>> GetLogsEstoque();
    }
}
