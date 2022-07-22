using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEPI.DTO
{
    public class ComprasDTO : IEntityTypeConfiguration<ComprasDTO>
    {
        public void Configure(EntityTypeBuilder<ComprasDTO> builder)
        {
            builder.Property(e => e.produtos).HasConversion(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
            v => JsonConvert.DeserializeObject<IList<PedidosProdutos>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            builder.Property(e => e.idPedido).HasConversion(
            x => JsonConvert.SerializeObject(x, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
            x => JsonConvert.DeserializeObject<IList<Pedidos>>(x, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [JsonField]
        public IList<PedidosProdutos> produtos { get; set; }
        public DateTime? dataCompra { get; set; }
        public decimal valorTotalCompra { get; set; }
        public bool ativo { get; set; }
        [JsonField]
        public IEnumerable<Pedidos> idPedido { get; set; }
    }

    public class Pedidos
    {
        public int idPedido { get; set; }
    }

    public class PedidosProdutos
    {
        public int idProduto { get; set; }
        public int quantidade { get; set; }
        public DateTime? validadeProduto { get; set; }
        public DateTime? validadeUso { get; set; }
    }
}
