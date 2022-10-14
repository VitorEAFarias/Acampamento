using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class CargosDAL : ICargosBLL
    {
        public readonly AppDbContextRH _context;
        public CargosDAL(AppDbContextRH context)
        {
            _context = context;
        }

        public async Task<CargosDTO> getCargo(int Id)
        {
            return await _context.rh_cargos.FindAsync(Id);
        }

        public async Task<IEnumerable<CargosDTO>> getCargos()
        {
            return await _context.rh_cargos.ToListAsync();
        }
    }
}
