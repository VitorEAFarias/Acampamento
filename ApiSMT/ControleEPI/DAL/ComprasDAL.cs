using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class ComprasDAL : IComprasBLL
    {
        public readonly AppDbContext _context;

        public ComprasDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var compraDelete = await _context.EPIcompras.FindAsync(Id);
            _context.EPIcompras.Remove(compraDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<ComprasDTO> getCompra(int Id)
        {
            return await _context.EPIcompras.FindAsync(Id);
        }

        public async Task<IEnumerable<ComprasDTO>> getCompras()
        {
            return await _context.EPIcompras.ToListAsync();
        }

        public async Task<ComprasDTO> Insert(ComprasDTO compra)
        {
            _context.EPIcompras.Add(compra);
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
            return await _context.EPIcompras.FromSqlRaw("SELECT * FROM compras where status = '" + status + "'").ToListAsync();
        }
    }
}
