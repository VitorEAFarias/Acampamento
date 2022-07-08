using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface ILogEstoqueDAL
    {
        Task<LogEstoqueDTO> Insert(LogEstoqueDTO logEstoque);
        Task<LogEstoqueDTO> GetLogEstoque(int Id);
        Task<IEnumerable<LogEstoqueDTO>> GetLogsEstoque();
    }
}
