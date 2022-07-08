using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class DepartamentosBLL : IDepartamentosDAL
    {
        public readonly AppDbContextRH _context;
        public DepartamentosBLL(AppDbContextRH context)
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
