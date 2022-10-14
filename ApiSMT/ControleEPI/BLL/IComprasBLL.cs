using System.Collections.Generic;
using System.Threading.Tasks;
using ControleEPI.DTO;

namespace ControleEPI.BLL
{
    public interface IComprasBLL
    {
        Task<ComprasDTO> Insert(ComprasDTO compra);
        Task<ComprasDTO> getCompra(int Id);
        Task<IEnumerable<ComprasDTO>> getCompras();
        Task<IEnumerable<ComprasDTO>> getStatusCompras(int status);
        Task Update(ComprasDTO compra);
        Task Delete(int id);
    }
}
