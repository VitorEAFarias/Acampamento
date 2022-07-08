using System;

namespace ControleEPI.DTO
{
    public class PedidosStatusDTO
    {
        public int id { get; set; }
        public int idPedido { get; set; }
        public int idUsuario { get; set; }
        public DateTime? data { get; set; }
        public int status { get; set; }
        public string descricao { get; set; }
        public string auxiliar { get; set; }
    }
}
