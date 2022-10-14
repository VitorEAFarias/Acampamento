using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DTO.E_Mail;
using ControleEPI.BLL;
using System;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe que encia e-mail
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailServiceBLL mailService;

        /// <summary>
        /// Construtor EmailController
        /// </summary>
        /// <param name="mailService"></param>
        public EmailController(IMailServiceBLL mailService)
        {
            this.mailService = mailService;
        }

        /// <summary>
        /// Post para enviar e-mail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMail([FromBody] EmailRequestDTO request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
