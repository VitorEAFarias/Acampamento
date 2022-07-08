using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL 
{
    public class EmpContratosBLL : IEmpContratosDAL
    {
        public readonly AppDbContextRH _context;
        public EmpContratosBLL(AppDbContextRH context)
        {
            _context = context;
        }

        public async Task<EmpContratosDTO> getContrato(int Id)
        {
            return await _context.rh_empregados_contratos.FindAsync(Id);
        }

        public async Task<IEnumerable<EmpContratosDTO>> getContratos()
        {
            return await _context.rh_empregados_contratos.ToListAsync();
        }

        public async Task<EmpContratosDTO> getEmpContrato(int IdEmpregado)
        {
            return await _context.rh_empregados_contratos.FromSqlRaw("SELECT * FROM rh_empregados_contratos where id_empregado = '" + IdEmpregado + "' AND contrato_atual = 1 AND contrato_principal = true").SingleOrDefaultAsync();
        }
    }
}
