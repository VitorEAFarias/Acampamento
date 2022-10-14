using ControleEPI.DTO.E_Mail;
using System.Threading.Tasks;

namespace ControleEPI.BLL
{
    public interface IMailServiceBLL
    {
        Task SendEmailAsync(EmailRequestDTO emailRequest);
    }
}
