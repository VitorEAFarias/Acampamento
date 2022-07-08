namespace ControleEPI.DTO
{
    public class ProdutosDTO
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int quantidade { get; set; }
        public string ca { get; set; }
        public decimal valor { get; set; }
        public int idFornecedor{ get; set; }
        public int idCategoria { get; set; }
    }
}
