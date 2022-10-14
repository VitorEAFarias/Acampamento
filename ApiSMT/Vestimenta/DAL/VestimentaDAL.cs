using Vestimenta.DTO._DbContext;
using Vestimenta.DTO;
using Vestimenta.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vestimenta.DAL
{
    public class VestimentaDAL : IVestimentaBLL
    {
        public readonly VestAppDbContext _context;

        public VestimentaDAL(VestAppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var statusDelete = await _context.VestVestimenta.FindAsync(Id);
            _context.VestVestimenta.Remove(statusDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<VestimentaDTO> getVestimenta(int Id)
        {
            return await _context.VestVestimenta.FindAsync(Id);
        }

        public async Task<VestimentaDTO> getNomeVestimenta(string Nome)
        {
            return await _context.VestVestimenta.FromSqlRaw("SELECT * FROM VestVestimenta WHERE nome = '" + Nome + "'").FirstOrDefaultAsync();
        }

        public async Task<IList<VestimentaDTO>> getItens(int idVestimenta)
        {
            return await _context.VestVestimenta.FromSqlRaw("SELECT * from VestVestimenta WHERE id = '" +idVestimenta+ "'").ToListAsync();
        }

        public async Task<IList<VestimentaDTO>> getVestimentas()
        {
            return await _context.VestVestimenta.ToListAsync();
        }

        public async Task<VestimentaDTO> Insert(VestimentaDTO vestimenta)
        {
            _context.VestVestimenta.Add(vestimenta);
            await _context.SaveChangesAsync();

            return vestimenta;
        }

        public async Task<VestimentaDTO> Update(VestimentaDTO vestimenta)
        {
            _context.ChangeTracker.Clear();

            _context.Entry(vestimenta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return vestimenta;
        }
    }
}
