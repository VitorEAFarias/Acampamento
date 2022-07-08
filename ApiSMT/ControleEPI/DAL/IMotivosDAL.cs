using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IMotivosDAL
    {
        Task<IEnumerable<MotivoDTO>> Get();
        Task<MotivoDTO> Get(int Id);
        Task<MotivoDTO> Insert(MotivoDTO motivo);
        Task Update(MotivoDTO motivo);
        Task Delete(int Id);
    }
}
