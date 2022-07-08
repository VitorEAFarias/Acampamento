using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;

namespace ControleEPI.DTO._DbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Produtos>();
            modelBuilder.AddJsonFields();
        }

        public DbSet<CategoriaDTO> categoria { get; set; }
        public DbSet<EpiVinculoDTO> epiVinculo { get; set; }
        public DbSet<FornecedorDTO> fornecedor { get; set; }
        public DbSet<MotivoDTO> motivos { get; set; }
        public DbSet<PedidosDTO> pedidos { get; set; }
        public DbSet<PedidosStatusDTO> pedidosStatus { get; set; }
        public DbSet<ProdutosDTO> produtos { get; set; }
        public DbSet<StatusDTO> status { get; set; }
        public DbSet<LogEstoqueDTO> logEstoque { get; set; }
    }
}
