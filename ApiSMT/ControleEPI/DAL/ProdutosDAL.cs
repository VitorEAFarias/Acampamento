using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class ProdutosDAL : IProdutosBLL
    {
        public readonly AppDbContext _context;
        public ProdutosDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProdutosDTO> Insert(ProdutosDTO produto)
        {
            _context.EPIprodutos.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<IEnumerable<ProdutosDTO>> getProdutos()
        {
            return await _context.EPIprodutos.ToListAsync();
        }

        public async Task<ProdutosDTO> getNomeProduto(string nome)
        {
            return await _context.EPIprodutos.FromSqlRaw("SELECT * FROM produtos WHERE nome = '" + nome + "'").FirstOrDefaultAsync();
        }

        public async Task<ProdutosDTO> getFornecedorProduto(int IdFornecedor)
        {
            return await _context.EPIprodutos.FromSqlRaw("SELECT * FROM produtos WHERE idFornecedor = '" + IdFornecedor + "'").FirstOrDefaultAsync();
        }

        public async Task<ProdutosDTO> getCategoriaProduto(int IdCategoria)
        {
            return await _context.EPIprodutos.FromSqlRaw("SELECT * FROM produtos WHERE idCategoria = '" + IdCategoria + "'").FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProdutosDTO>> getCategoriaProdutos(int IdCategoria)
        {
            return await _context.EPIprodutos.FromSqlRaw("SELECT * FROM produtos WHERE idCategoria = '" + IdCategoria + "'").ToListAsync();
        }

        public async Task<ProdutosDTO> getProduto(int Id)
        {
            return await _context.EPIprodutos.FindAsync(Id);
        }

        public async Task Update(ProdutosDTO produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var produtoDelete = await _context.EPIprodutos.FindAsync(Id);
            _context.EPIprodutos.Remove(produtoDelete);

            await _context.SaveChangesAsync();
        }
    }
}
