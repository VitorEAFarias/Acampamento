using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class StatusDAL : IStatusBLL
    {
        public readonly AppDbContext _context;
        public StatusDAL(AppDbContext context)
        {
            _context = context;
        }
        public async Task Delete(int Id)
        {
            var statusDelete = await _context.EPIstatus.FindAsync(Id);
            _context.EPIstatus.Remove(statusDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StatusDTO>> getTodosStatus()
        {
            return await _context.EPIstatus.ToListAsync();
        }

        public async Task<StatusDTO> getStatus(int Id)
        {
            return await _context.EPIstatus.FindAsync(Id);
        }

        public async Task<StatusDTO> Insert(StatusDTO status)
        {
            _context.EPIstatus.Add(status);
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
