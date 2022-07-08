using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class PedidosStatusBLL : IPedidosStatusDAL
    {
        public readonly AppDbContext _context;
        public PedidosStatusBLL(AppDbContext context)
        {
            _context = context;
        }
        public async Task Delete(int Id)
        {
            var PedidoStatusDelete = await _context.pedidosStatus.FindAsync(Id);
            _context.pedidosStatus.Remove(PedidoStatusDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PedidosStatusDTO>> Get()
        {
            return await _context.pedidosStatus.ToListAsync();
        }

        public async Task<PedidosStatusDTO> Get(int Id)
        {
            return await _context.pedidosStatus.FindAsync(Id);
        }

        public async Task<PedidosStatusDTO> Insert(PedidosStatusDTO pedidoStatus)
        {
            _context.pedidosStatus.Add(pedidoStatus);
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
