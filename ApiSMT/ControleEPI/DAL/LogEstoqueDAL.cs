using ControleEPI.DTO._DbContext;
using ControleEPI.DTO;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public class LogEstoqueDAL : ILogEstoqueBLL
    {
        public readonly AppDbContext _context;
        public LogEstoqueDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LogEstoqueDTO> Insert(LogEstoqueDTO logEstoque)
        {
            _context.EPIlogEstoque.Add(logEstoque);
            await _context.SaveChangesAsync();

            return logEstoque;
        }

        public async Task<IEnumerable<LogEstoqueDTO>> GetLogsEstoque()
        {
            return await _context.EPIlogEstoque.ToListAsync();
        }

        public async Task<LogEstoqueDTO> GetLogEstoque(int Id)
        {
            return await _context.EPIlogEstoque.FindAsync(Id);
        }
    }
}
