using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleEPI.DAL;
using ControleEPI.DTO.FromBody;
using ApiSMT.Utilitários;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace ApiSMT.Controllers.ControllersEPI
{
    /// <summary>
    /// Classe de login do sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConUserDAL _conuser;
        private IConfiguration _config;

        /// <summary>
        /// Construtor LoginController
        /// </summary>
        /// <param name="conuser"></param>
        /// <param name="config"></param>
        public LoginController(IConUserDAL conuser, IConfiguration config)
        {
            _conuser = conuser;
            _config = config;
        }

        /// <summary>
        /// Verifica credenciais de usuario
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginDTO login)
        {
            var doc = await _conuser.GetDoc();

            string cpf = "";
            string senha = "";
            string usuario = "";
            string email = "";
            int id = 0;
            var tokenString = "";
            foreach (var item in doc)
            {
                if (item.numero == login.CPF)
                {
                    cpf = item.numero;
                    if (cpf != "")
                    {
                        var senhas = await _conuser.GetSenha(item.id_empregado);
                        var empregado = await _conuser.GetEmp(senhas.id_empregado);
                        var emailCorp = await _conuser.GetEmpCont(item.id_empregado);
                        usuario = empregado.nome;
                        id = empregado.id;
                        email = emailCorp.valor;

                        GerarMD5 md5 = new GerarMD5();

                        var senhaMD5 = md5.GeraMD5(login.Senha);
                        if (senhas.senha == senhaMD5)
                        {
                            senha = senhas.senha;
                            //var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                            //var _issuer = _config["Jwt:Issuer"];
                            //var _audience = _config["Jwt:Audience"];

                            //var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

                            //var tokeOptions = new JwtSecurityToken(
                            //    issuer: _issuer,
                            //    audience: _audience,
                            //    claims: new List<Claim>(),
                            //    expires: DateTime.Now.AddMinutes(5),
                            //    signingCredentials: signinCredentials);

                            //tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                            break;                           
                        }
                    }
                }
            }

            if (cpf != "")
            {
                if (senha != "")
                {
                    return Ok(new { id = id, nome = usuario, email = email, data = true, message = usuario+ " Logado com sucesso!!!", Token = tokenString });
                }
                else
                {
                    return BadRequest(new { data = false, message = "Senha incorreta" });
                }
            }
            else
            {
                return BadRequest(new { data = false, message = "Usuario não encontrado" });
            }            
        }
    }
}
