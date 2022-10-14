using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class PedidosDAL : IPedidosBLL
    {
        public readonly AppDbContext _context;
        public PedidosDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PedidosDTO> Insert(PedidosDTO pedidos)
        {
            _context.EPIpedidos.Add(pedidos);
            await _context.SaveChangesAsync();

            return pedidos;
        }

        public async Task<IList<PedidosDTO>> getTodosPedidos()
        {
            return await _context.EPIpedidos.ToListAsync();            
        }

        public async Task<IList<PedidosDTO>> getPedidosUsuario(int id)
        {
            return await _context.EPIpedidos.FromSqlRaw("SELECT * FROM pedidos where idUsuario = '" + id + "'").ToListAsync();
        }

        public async Task<IList<PedidosDTO>> getPedidos(int idPedido)
        {
            return await _context.EPIpedidos.FromSqlRaw("SELECT * FROM pedidos WHERE id = '" + idPedido + "'").ToListAsync();
        }

        public async Task<PedidosDTO> getPedido(int Id)
        {
            return await _context.EPIpedidos.FindAsync(Id);
        }

        public async Task Update(PedidosDTO pedido)
        {
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var pedidoDelete = await _context.EPImotivos.FindAsync(Id);
            _context.EPImotivos.Remove(pedidoDelete);

            await _context.SaveChangesAsync();
        }
    }
}