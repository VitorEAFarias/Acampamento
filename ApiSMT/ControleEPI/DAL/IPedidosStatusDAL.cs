using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IPedidosStatusDAL
    {
        Task<IEnumerable<PedidosStatusDTO>> Get();
        Task<PedidosStatusDTO> Get(int Id);
        Task<PedidosStatusDTO> Insert(PedidosStatusDTO pedidoStatus);
        Task Update(PedidosStatusDTO pedidoStatus);
        Task Delete(int Id);
    }
}
