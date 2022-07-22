using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class PedidosBLL : IPedidosDAL
    {
        public readonly AppDbContext _context;
        public PedidosBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PedidosDTO> Insert(PedidosDTO pedidos)
        {
            _context.pedidos.Add(pedidos);
            await _context.SaveChangesAsync();

            return pedidos;
        }

        public async Task<IList<PedidosDTO>> getPedidos()
        {
            return await _context.pedidos.ToListAsync();            
        }

        public async Task<IList<PedidosDTO>> getPedidosUsuario(int id)
        {
            return await _context.pedidos.FromSqlRaw("SELECT * FROM pedidos where idUsuario = '" + id + "'").ToListAsync();
        }

        public async Task<PedidosDTO> getPedido(int Id)
        {
            return await _context.pedidos.FindAsync(Id);
        }

        public async Task Update(PedidosDTO pedido)
        {
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var pedidoDelete = await _context.motivos.FindAsync(Id);
            _context.motivos.Remove(pedidoDelete);

            await _context.SaveChangesAsync();
        }
    }
}