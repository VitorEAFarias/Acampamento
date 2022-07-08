using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class FornecedorBLL : IFornecedoresDAL
    {
        public readonly AppDbContext _context;
        public FornecedorBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FornecedorDTO> Insert(FornecedorDTO fornecedor)
        {
            _context.fornecedor.Add(fornecedor);
            await _context.SaveChangesAsync();

            return fornecedor;
        }

        public async Task<IEnumerable<FornecedorDTO>> getFornecedores()
        {
            return await _context.fornecedor.ToListAsync();
        }

        public async Task<FornecedorDTO> getNomeFornecedor(string nome)
        {
            return await _context.fornecedor.FromSqlRaw("SELECT * FROM fornecedor where nome = '"+nome+"'").FirstOrDefaultAsync();
        }

        public async Task<FornecedorDTO> getFornecedor(int Id)
        {
            return await _context.fornecedor.FindAsync(Id);
        }

        public async Task Update(FornecedorDTO pedido)
        {
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var fornecedorDelete = await _context.fornecedor.FindAsync(Id);
            _context.fornecedor.Remove(fornecedorDelete);

            await _context.SaveChangesAsync();
        }
    }
}
