using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class StatusBLL : IStatusDAL
    {
        public readonly AppDbContext _context;
        public StatusBLL(AppDbContext context)
        {
            _context = context;
        }
        public async Task Delete(int Id)
        {
            var statusDelete = await _context.status.FindAsync(Id);
            _context.status.Remove(statusDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StatusDTO>> getTodosStatus()
        {
            return await _context.status.ToListAsync();
        }

        public async Task<StatusDTO> getStatus(int Id)
        {
            return await _context.status.FindAsync(Id);
        }

        public async Task<StatusDTO> Insert(StatusDTO status)
        {
            _context.status.Add(status);
            await _context.SaveChangesAsync();

            return status;
        }

        public async Task Update(StatusDTO status)
        {
            _context.Entry(status).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
