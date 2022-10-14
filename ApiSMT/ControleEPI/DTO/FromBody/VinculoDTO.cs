using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ControleEPI.DTO.FromBody
{
    public class VinculoDTO
    {
        public void Configure(EntityTypeBuilder<VinculoDTO> builder)
        {
            builder.Property(e => e.produto).HasConversion(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
            v => JsonConvert.DeserializeObject<IList<ProdutoVinculo>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        public int usuario { get; set; }
        public int usuarioLogado { get; set; }
        [JsonField]
        public IList<ProdutoVinculo> produto { get; set; }

        public class ProdutoVinculo
        {
            public int id { get; set; }
            public string nome { get; set; }
            public int quantidade { get; set; }
        }
    }
}
