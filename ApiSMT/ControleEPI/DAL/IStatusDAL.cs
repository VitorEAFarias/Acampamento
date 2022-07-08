using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IStatusDAL
    {
        Task<IEnumerable<StatusDTO>> getTodosStatus();
        Task<StatusDTO> getStatus(int Id);
        Task<StatusDTO> Insert(StatusDTO status);
        Task Update(StatusDTO status);
        Task Delete(int Id);
    }
}
