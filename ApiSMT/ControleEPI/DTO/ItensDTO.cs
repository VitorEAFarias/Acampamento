using System;

namespace ControleEPI.DTO
{
    public class ItensDTO
    {
        public int id { get; set; }
        public int idProduto { get; set; }
        public DateTime validade { get; set; }
        public string descricao { get; set; }
        public string codigoBarra { get; set; }
        public DateTime data { get; set; }
    }
}
