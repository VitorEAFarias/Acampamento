using System;

namespace ControleEPI.DTO
{
    public class EpiVinculoDTO
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idItem { get; set; }
        public DateTime? dataVinculo { get; set; }
        public int ativo { get; set; }
        public string token { get; set; }
        public int idUsuarioVinculo { get; set; }
    }
}
