using System.Collections.Generic;
using System.Threading.Tasks;
using Vestimenta.DTO;

namespace Vestimenta.BLL
{
    public interface IVestimentaBLL
    {
        Task<VestimentaDTO> Insert(VestimentaDTO vestimenta);
        Task<VestimentaDTO> getVestimenta(int Id);
        Task<VestimentaDTO> getNomeVestimenta(string Nome);
        Task<IList<VestimentaDTO>> getVestimentas();
        Task<IList<VestimentaDTO>> getItens(int idVestimenta);
        Task<VestimentaDTO> Update(VestimentaDTO vestimenta);
        Task Delete(int id);
    }
}
