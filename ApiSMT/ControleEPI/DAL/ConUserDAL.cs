using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class ConUserDAL : IConUserBLL
    {
        public readonly AppDbContextRH _context;
        public ConUserDAL(AppDbContextRH context)
        {
            _context = context;
        }

        public async Task<List<object>> getSuperioresColaboradores(int idEmp)
        {
            var contratos = await _context.rh_empregados_contratos.FromSqlRaw("SELECT * FROM rh_empregados_contratos WHERE contrato_atual = 1 AND contrato_principal = 1 " +
                "AND id_empregado_superior = '"+idEmp+"'").ToListAsync();

            var nomeSupervisor = await _context.rh_empregados.FromSqlRaw("SELECT * FROM rh_empregados WHERE id = '"+idEmp+"'").FirstOrDefaultAsync();

            List<object> colaboradores = new List<object>();

            foreach (var item in contratos)
            {
                var colaborador = await _context.rh_empregados.FromSqlRaw("SELECT * FROM rh_empregados WHERE id = '" + item.id_empregado + "' AND ativo = 1").FirstOrDefaultAsync();

                if (colaborador != null)
                {
                    colaboradores.Add(new
                    {
                        id = colaborador.id,
                        nome = colaborador.nome
                    });
                }                
            } 

            var supervisor = new {
                id = nomeSupervisor.id,
                nome = nomeSupervisor.nome
            };

            colaboradores.Add(supervisor);

            return colaboradores;
        }

        public async Task<EmpContatoDTO> getEmail(int idEmpregado)
        {
            return await _context.rh_empregados_contatos.FromSqlRaw("SELECT * FROM rh_empregados_contatos WHERE id_empregado = '"+idEmpregado+"' AND tipo_contato = 13").FirstOrDefaultAsync();
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
