using Microsoft.EntityFrameworkCore;

namespace ControleEPI.DTO._DbContext
{
    public class AppDbContextRH : DbContext
    {
        public AppDbContextRH(DbContextOptions<AppDbContextRH> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<EmpregadoDTO> rh_empregados { get; set; }
        public DbSet<DocumentoDTO> rh_empregados_documentos { get; set; }
        public DbSet<SenhaDTO> rh_empregados_senhas { get; set; }
        public DbSet<CargosDTO> rh_cargos { get; set; }
        public DbSet<DepartamentosDTO> rh_departamentos { get; set; }
        public DbSet<EmpContratosDTO> rh_empregados_contratos { get; set; }
        public DbSet<EmpContatoDTO> rh_empregados_contatos { get; set; }
    }
}
