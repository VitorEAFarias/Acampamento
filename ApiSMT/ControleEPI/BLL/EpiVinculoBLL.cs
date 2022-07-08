using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class EpiVinculoBLL : IEpiVinculoDAL
    {
        public readonly AppDbContext _context;
        public EpiVinculoBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var epiVinculo = await _context.epiVinculo.FindAsync(Id);
            _context.epiVinculo.Remove(epiVinculo);

            await _context.SaveChangesAsync();
        }

        public async Task<EpiVinculoDTO> GetProdutoVinculo(int IdProduto)
        {
            return await _context.epiVinculo.FromSqlRaw("SELECT * FROM produtos where produto = '" + IdProduto + "'").FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EpiVinculoDTO>> GetVinculos()
        {
            return await _context.epiVinculo.ToListAsync();
        }

        public async Task<IEnumerable<EpiVinculoDTO>> GetUsuarioVinculo(int Id)
        {
            return await _context.epiVinculo.FromSqlRaw("SELECT * FROM epiVinculo where idUsuario = '" + Id + "' AND ativo = 1").ToListAsync();
        }

        public async Task<EpiVinculoDTO> GetVinculo(int Id)
        {
            return await _context.epiVinculo.FindAsync(Id);
        }

        public async Task<EpiVinculoDTO> Insert(EpiVinculoDTO epivinculo)
        {
            _context.epiVinculo.Add(epivinculo);
            await _context.SaveChangesAsync();

            return epivinculo;
        }

        public async Task Update(EpiVinculoDTO epivinculo)
        {
            _context.Entry(epivinculo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
