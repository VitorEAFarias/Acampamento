using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IPedidosBLL
    {        
        Task<PedidosDTO> Insert(PedidosDTO pedido);
        Task<PedidosDTO> getPedido(int Id);
        Task<IList<PedidosDTO>> getPedidos(int idPedido);
        Task<IList<PedidosDTO>> getTodosPedidos();
        Task<IList<PedidosDTO>> getPedidosUsuario(int Id);
        Task Update(PedidosDTO pedido);
        Task Delete(int id);
    }
}