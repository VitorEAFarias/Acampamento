using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class MotivosDAL : IMotivosBLL
    {
        public readonly AppDbContext _context;
        public MotivosDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MotivoDTO> Insert(MotivoDTO motivo)
        {
            _context.EPImotivos.Add(motivo);
            await _context.SaveChangesAsync();

            return motivo;
        }        

        public async Task<IEnumerable<MotivoDTO>> getMotivos()
        {
            return await _context.EPImotivos.ToListAsync();
        }

        public async Task<MotivoDTO> getMotivo(int Id)
        {
            return await _context.EPImotivos.FindAsync(Id);
        }        

        public async Task Update(MotivoDTO motivo)
        {
            _context.Entry(motivo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var motivoDelete = await _context.EPImotivos.FindAsync(Id);
            _context.EPImotivos.Remove(motivoDelete);

            await _context.SaveChangesAsync();
        }
    }
}
