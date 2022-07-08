using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.DAL
{
    public interface IPedidosDAL
    {        
        Task<PedidosDTO> Insert(PedidosDTO pedido);
        Task<PedidosDTO> getPedido(int Id);
        Task<IList<PedidosDTO>> getPedidos();
        Task Update(PedidosDTO pedido);
        Task Delete(int id);
    }
}