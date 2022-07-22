using ControleEPI.DTO.E_Mail;
using System.Threading.Tasks;

namespace ControleEPI.DAL
{
    public interface IMailServiceDAL
    {
        Task SendEmailAsync(EmailRequestDTO emailRequest);
    }
}
