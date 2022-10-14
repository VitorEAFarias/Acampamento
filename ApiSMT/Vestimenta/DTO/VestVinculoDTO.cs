using System;

namespace Vestimenta.DTO
{
    public class VestVinculoDTO
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idUsuarioVinculo { get; set; }
        public int idVestimenta { get; set; }
        public DateTime dataVinculo { get; set; }
        public int status { get; set; }
        public string tamanhoVestVinculo { get; set; }
    }
}
