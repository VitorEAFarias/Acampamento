using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class CargosBLL : ICargosDAL
    {
        public readonly AppDbContextRH _context;
        public CargosBLL(AppDbContextRH context)
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
