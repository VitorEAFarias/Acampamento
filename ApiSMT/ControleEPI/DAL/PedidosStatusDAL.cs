using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class PedidosStatusDAL : IPedidosStatusBLL
    {
        public readonly AppDbContext _context;
        public PedidosStatusDAL(AppDbContext context)
        {
            _context = context;
        }
        public async Task Delete(int Id)
        {
            var PedidoStatusDelete = await _context.EPIpedidosStatus.FindAsync(Id);
            _context.EPIpedidosStatus.Remove(PedidoStatusDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PedidosStatusDTO>> Get()
        {
            return await _context.EPIpedidosStatus.ToListAsync();
        }

        public async Task<IList<PedidosStatusDTO>> getPedidosCompras(int idStatus)
        {
            return await _context.EPIpedidosStatus.FromSqlRaw("SELECT * FROM pedidosStatus WHERE status = '" + idStatus + "'").ToListAsync();
        }

        public async Task<PedidosStatusDTO> Get(int Id)
        {
            return await _context.EPIpedidosStatus.FindAsync(Id);
        }

        public async Task<PedidosStatusDTO> Insert(PedidosStatusDTO pedidoStatus)
        {
            _context.EPIpedidosStatus.Add(pedidoStatus);
            await _context.SaveChangesAsync();

            return pedidoStatus;
        }

        public async Task Update(PedidosStatusDTO pedidoStatus)
        {
            _context.Entry(pedidoStatus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
