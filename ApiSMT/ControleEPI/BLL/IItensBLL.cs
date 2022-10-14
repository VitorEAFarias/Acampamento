using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IItensBLL
    {
        Task<ItensDTO> Insert(ItensDTO item);
        Task<ItensDTO> getItem(int Id);
        Task<IEnumerable<ItensDTO>> getItens();
        Task Update(ItensDTO item);
        Task Delete(int id);
    }
}
