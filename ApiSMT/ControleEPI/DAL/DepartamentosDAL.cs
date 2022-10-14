using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class DepartamentosDAL : IDepartamentosBLL
    {
        public readonly AppDbContextRH _context;
        public DepartamentosDAL(AppDbContextRH context)
        {
            _context = context;
        }
        public async Task<DepartamentosDTO> getDepartamento(int Id)
        {
            return await _context.rh_departamentos.FindAsync(Id);
        }

        public async Task<IEnumerable<DepartamentosDTO>> getDepartamentos()
        {
            return await _context.rh_departamentos.ToListAsync();
        }        
    }
}
