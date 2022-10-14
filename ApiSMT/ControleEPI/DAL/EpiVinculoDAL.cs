using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class EpiVinculoDAL : IEpiVinculoBLL
    {
        public readonly AppDbContext _context;
        public EpiVinculoDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var epiVinculo = await _context.EPIepiVinculo.FindAsync(Id);
            _context.EPIepiVinculo.Remove(epiVinculo);

            await _context.SaveChangesAsync();
        }

        public async Task<EpiVinculoDTO> GetProdutoVinculo(int IdProduto)
        {
            return await _context.EPIepiVinculo.FromSqlRaw("SELECT * FROM produtos where produto = '" + IdProduto + "'").FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EpiVinculoDTO>> GetVinculos()
        {
            return await _context.EPIepiVinculo.ToListAsync();
        }

        public async Task<IEnumerable<EpiVinculoDTO>> GetUsuarioVinculo(int Id)
        {
            var query =  await _context.EPIepiVinculo.FromSqlRaw("SELECT * FROM epiVinculo where idUsuario = '" + Id + "'").ToListAsync();
            return query;
        }

        public async Task<IEnumerable<EpiVinculoDTO>> GetUsuarioVinculoStatus(int Id, int status)
        {
            var query = await _context.EPIepiVinculo.FromSqlRaw("SELECT * FROM epiVinculo where idUsuario = '" + Id + "' AND ativo = '"+status+"'").ToListAsync();
            return query;
        }

        public async Task<EpiVinculoDTO> GetVinculo(int Id)
        {
            return await _context.EPIepiVinculo.FindAsync(Id);
        }

        public async Task<EpiVinculoDTO> Insert(EpiVinculoDTO epivinculo)
        {
            _context.EPIepiVinculo.Add(epivinculo);
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
