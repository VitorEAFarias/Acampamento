using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ControleEPI.DTO.E_Mail
{
    public class EmailRequestDTO : IEntityTypeConfiguration<EmailRequestDTO>
    {
        public void Configure(EntityTypeBuilder<EmailRequestDTO> builder)
        {
            builder.Property(e => e.conteudo).HasConversion(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
            v => JsonConvert.DeserializeObject<IList<Email>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        public string EmailDe { get; set; }
        public string EmailPara { get; set; }
        public string Assunto { get; set; }
        [JsonField]
        public IList<Email> conteudo { get; set; }
    }

    public class Email
    {
        public string nome { get; set; }
        public string statusPedido { get; set; }
    }
}
