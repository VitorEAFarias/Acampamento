using System;

namespace ControleEPI.DTO
{
    public class EpiVinculoDTO
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string produto { get; set; }
        public DateTime? dataVinculo { get; set; }
        public bool ativo { get; set; }
        public string token { get; set; }
    }
}
