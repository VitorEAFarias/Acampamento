using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class ProdutosBLL : IProdutosDAL
    {
        public readonly AppDbContext _context;
        public ProdutosBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProdutosDTO> Insert(ProdutosDTO produto)
        {
            _context.produtos.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<IEnumerable<ProdutosDTO>> getProdutos()
        {
            return await _context.produtos.ToListAsync();
        }

        public async Task<ProdutosDTO> getNomeProduto(string nome)
        {
            return await _context.produtos.FromSqlRaw("SELECT * FROM produtos where nome = '"+nome+"'").FirstOrDefaultAsync();
        }

        public async Task<ProdutosDTO> getFornecedorProduto(int IdFornecedor)
        {
            return await _context.produtos.FromSqlRaw("SELECT * FROM produtos where idFornecedor = '" + IdFornecedor + "'").FirstOrDefaultAsync();
        }

        public async Task<ProdutosDTO> getCategoriaProduto(int IdCategoria)
        {
            return await _context.produtos.FromSqlRaw("SELECT * FROM produtos where idCategoria = '" + IdCategoria + "'").FirstOrDefaultAsync();
        }

        public async Task<ProdutosDTO> getProduto(int Id)
        {
            return await _context.produtos.FindAsync(Id);
        }

        public async Task Update(ProdutosDTO produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var produtoDelete = await _context.produtos.FindAsync(Id);
            _context.produtos.Remove(produtoDelete);

            await _context.SaveChangesAsync();
        }
    }
}
