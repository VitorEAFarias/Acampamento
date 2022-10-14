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

        public DbSet<CategoriaDTO> EPIcategoria { get; set; }
        public DbSet<EpiVinculoDTO> EPIepiVinculo { get; set; }
        public DbSet<FornecedorDTO> EPIfornecedor { get; set; }
        public DbSet<MotivoDTO> EPImotivos { get; set; }
        public DbSet<PedidosDTO> EPIpedidos { get; set; }
        public DbSet<PedidosStatusDTO> EPIpedidosStatus { get; set; }
        public DbSet<ProdutosDTO> EPIprodutos { get; set; }
        public DbSet<StatusDTO> EPIstatus { get; set; }
        public DbSet<LogEstoqueDTO> EPIlogEstoque { get; set; }
        public DbSet<ComprasDTO> EPIcompras { get; set; }
        public DbSet<ItensDTO> EPIitens { get; set; }
    }
}
