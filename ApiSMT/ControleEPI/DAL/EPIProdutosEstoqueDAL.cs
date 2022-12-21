﻿using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ControleEPI.DAL
{
    public class EPIProdutosEstoqueDAL : IEPIProdutosEstoqueBLL
    {
        public readonly AppDbContext _context;
        public EPIProdutosEstoqueDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EPIProdutosEstoqueDTO> Insert(EPIProdutosEstoqueDTO produto)
        {
            _context.EPIProdutosEstoque.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<EPIProdutosEstoqueDTO> getProdutoEstoque(int id)
        {
            return await _context.EPIProdutosEstoque.FromSqlRaw("SELECT * FROM EPIProdutosEstoque WHERE id = '" + id + "'").OrderBy(x => x.id).FirstOrDefaultAsync();
        }

        public async Task<EPIProdutosEstoqueDTO> getProdutoEstoqueTamanho(int id, int idTamanho)
        {
            return await _context.EPIProdutosEstoque.FromSqlRaw("SELECT * FROM EPIProdutosEstoque WHERE idProduto = '" + id + "' AND " +
                "idTamanho = '"+ idTamanho + "'").OrderBy(x => x.id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EPIProdutosEstoqueDTO>> getProdutosEstoque()
        {
            return await _context.EPIProdutosEstoque.ToListAsync();
        }

        public async Task Update(EPIProdutosEstoqueDTO produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
