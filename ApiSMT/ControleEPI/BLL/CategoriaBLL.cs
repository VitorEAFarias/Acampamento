using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class CategoriaBLL : ICategoriasDAL
    {
        public readonly AppDbContext _context;

        public CategoriaBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var categoriaDelete = await _context.categoria.FindAsync(Id);
            _context.categoria.Remove(categoriaDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<CategoriaDTO> getNomeCategoria(string nome)
        {
            return await _context.categoria.FromSqlRaw("SELECT * FROM categoria where nome = '" + nome + "'").FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CategoriaDTO>> getCategorias()
        {
            return await _context.categoria.ToListAsync();
        }

        public async Task<CategoriaDTO> getCategoria(int Id)
        {
            return await _context.categoria.FindAsync(Id);
        }

        public async Task<CategoriaDTO> Insert(CategoriaDTO categoria)
        {
            _context.categoria.Add(categoria);
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
