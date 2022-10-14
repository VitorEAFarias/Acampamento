using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class ItensDAL : IItensBLL
    {
        public readonly AppDbContext _context;
        public ItensDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItensDTO> Insert(ItensDTO item)
        {
            _context.EPIitens.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<IEnumerable<ItensDTO>> getItens()
        {
            return await _context.EPIitens.ToListAsync();
        }

        public async Task<ItensDTO> getItem(int Id)
        {
            return await _context.EPIitens.FindAsync(Id);
        }

        public async Task Update(ItensDTO item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var itemDelete = await _context.EPIitens.FindAsync(Id);
            _context.EPIitens.Remove(itemDelete);

            await _context.SaveChangesAsync();
        }
    }
}
