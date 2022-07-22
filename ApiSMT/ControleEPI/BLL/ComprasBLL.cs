using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class ComprasBLL : IComprasDAL
    {
        public readonly AppDbContext _context;

        public ComprasBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var compraDelete = await _context.compras.FindAsync(Id);
            _context.compras.Remove(compraDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<ComprasDTO> getCompra(int Id)
        {
            return await _context.compras.FindAsync(Id);
        }

        public async Task<IEnumerable<ComprasDTO>> getCompras()
        {
            return await _context.compras.ToListAsync();
        }

        public async Task<ComprasDTO> Insert(ComprasDTO compra)
        {
            _context.compras.Add(compra);
            await _context.SaveChangesAsync();

            return compra;
        }

        public async Task Update(ComprasDTO compra)
        {
            _context.Entry(compra).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ComprasDTO>> getStatusCompras(int status)
        {
            return await _context.compras.FromSqlRaw("SELECT * FROM compras where status = '" + status + "'").ToListAsync();
        }
    }
}
