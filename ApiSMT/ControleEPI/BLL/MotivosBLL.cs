using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class MotivosBLL : IMotivosDAL
    {
        public readonly AppDbContext _context;
        public MotivosBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MotivoDTO> Insert(MotivoDTO motivo)
        {
            _context.motivos.Add(motivo);
            await _context.SaveChangesAsync();

            return motivo;
        }        

        public async Task<IEnumerable<MotivoDTO>> Get()
        {
            return await _context.motivos.ToListAsync();
        }

        public async Task<MotivoDTO> Get(int Id)
        {
            return await _context.motivos.FindAsync(Id);
        }        

        public async Task Update(MotivoDTO motivo)
        {
            _context.Entry(motivo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var motivoDelete = await _context.motivos.FindAsync(Id);
            _context.motivos.Remove(motivoDelete);

            await _context.SaveChangesAsync();
        }
    }
}
