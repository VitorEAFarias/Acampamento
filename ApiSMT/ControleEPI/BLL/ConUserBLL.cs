using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class ConUserBLL : IConUserDAL
    {
        public readonly AppDbContextRH _context;
        public ConUserBLL(AppDbContextRH context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentoDTO>> GetDoc()
        {
            return await _context.rh_empregados_documentos.FromSqlRaw("SELECT * FROM rh_empregados_documentos WHERE tipo_documento = 2").ToListAsync();
        }

        public async Task<IEnumerable<EmpregadoDTO>> GetColaboradores()
        {
            return await _context.rh_empregados.FromSqlRaw("SELECT id, nome, matricula FROM rh_empregados WHERE ativo = 1").ToListAsync();
        }

        public async Task<EmpregadoDTO> GetEmp(int Id)
        {
            return await _context.rh_empregados.FindAsync(Id);
        }

        public async Task<SenhaDTO> GetSenha(int Id)
        {
            return await _context.rh_empregados_senhas.FromSqlRaw("SELECT * FROM rh_empregados_senhas WHERE id_empregado = '"+Id+"' AND ativo = 1").FirstOrDefaultAsync();           
        }

        public async Task<SenhaDTO> Get(int Id)
        {
            return await _context.rh_empregados_senhas.FindAsync(Id);
        }

        public async Task<EmpContatoDTO> GetEmpCont(int Id) 
        {
            return await _context.rh_empregados_contatos.FromSqlRaw("SELECT * FROM rh_empregados_contatos WHERE tipo_contato = 13").FirstOrDefaultAsync();
        }
    }
}
