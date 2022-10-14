using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class CategoriaDAL : ICategoriasBLL
    {
        public readonly AppDbContext _context;

        public CategoriaDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var categoriaDelete = await _context.EPIcategoria.FindAsync(Id);
            _context.EPIcategoria.Remove(categoriaDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<CategoriaDTO> getNomeCategoria(string nome)
        {
            return await _context.EPIcategoria.FromSqlRaw("SELECT * FROM categoria where nome = '" + nome + "'").FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CategoriaDTO>> getCategorias()
        {
            return await _context.EPIcategoria.ToListAsync();
        }

        public async Task<CategoriaDTO> getCategoria(int Id)
        {
            return await _context.EPIcategoria.FindAsync(Id);
        }

        public async Task<CategoriaDTO> Insert(CategoriaDTO categoria)
        {
            _context.EPIcategoria.Add(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task Update(CategoriaDTO categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
