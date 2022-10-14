using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IPedidosStatusBLL
    {
        Task<IEnumerable<PedidosStatusDTO>> Get();
        Task<IList<PedidosStatusDTO>> getPedidosCompras(int idStatus);
        Task<PedidosStatusDTO> Get(int Id);
        Task<PedidosStatusDTO> Insert(PedidosStatusDTO pedidoStatus);
        Task Update(PedidosStatusDTO pedidoStatus);
        Task Delete(int Id);
    }
}
