using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public class LogEstoqueBLL : ILogEstoqueDAL
    {
        public readonly AppDbContext _context;
        public LogEstoqueBLL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LogEstoqueDTO> Insert(LogEstoqueDTO logEstoque)
        {
            _context.logEstoque.Add(logEstoque);
            await _context.SaveChangesAsync();

            return logEstoque;
        }

        public async Task<IEnumerable<LogEstoqueDTO>> GetLogsEstoque()
        {
            return await _context.logEstoque.ToListAsync();
        }

        public async Task<LogEstoqueDTO> GetLogEstoque(int Id)
        {
            return await _context.logEstoque.FindAsync(Id);
        }
    }
}
