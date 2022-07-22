using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ControleEPI.DAL;
using ControleEPI.DTO.E_Mail;

namespace ApiSMT.Utilitários
{
    /// <summary>
    /// Classe de envio de e-mail
    /// </summary>
    public class MailService : IMailServiceDAL
    {
        private readonly EmailSettingsDTO _mailSettings;

        /// <summary>
        /// Construtor MailService
        /// </summary>
        /// <param name="emailSettings"></param>
        public MailService(IOptions<EmailSettingsDTO> emailSettings)
        {
            _mailSettings = emailSettings.Value;
        }

        /// <summary>
        /// Função para enviar e-mail
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(EmailRequestDTO mailRequest)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            EncryptDecrypt crypt = new EncryptDecrypt();

            var hash = crypt.Decrypt(_mailSettings.Password);
            string ConteudoEmail = string.Empty;            

            foreach (var item in mailRequest.conteudo)
            {
                ConteudoEmail += "<p> " + item.nome + " </p>";
                ConteudoEmail += "<p> " + item.statusPedido + " </p>";
            }

            var content = "<html>" +
                                "<head>" +
                                    "<meta http - equiv = 'Content-Type' content = 'text/html; charset=UTF-8' />" +
                                    "<meta name = 'viewport' content = 'width=device-width, initial-scale=1.0' />" +
                                    "<meta http - equiv = 'X-UA-Compatible' content = 'IE=edge' >" +
                                "</head >" +
                                "<body style = 'margin: 0; padding: 0;' >" +
                                    ConteudoEmail +                                
                                "</body>" +
                            "</html>";            

            message.From = new MailAddress(mailRequest.EmailDe, _mailSettings.DisplayName);
            message.To.Add(new MailAddress(mailRequest.EmailPara));
            message.Subject = mailRequest.Assunto;
            message.IsBodyHtml = true;
            message.Body = content;

            smtp.Port = _mailSettings.Port;
            smtp.Host = _mailSettings.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_mailSettings.usuario, hash);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            await smtp.SendMailAsync(message);
        }
    }
}