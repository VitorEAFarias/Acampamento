using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ControleEPI.DTO._DbContext;
using ControleEPI.DAL;
using ControleEPI.BLL;
using Vestimenta.DTO._DbContext;
using Vestimenta.DAL;
using Vestimenta.BLL;
using Microsoft.EntityFrameworkCore;
using ApiSMT.Utilitários;
using System.Reflection;
using System.IO;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiSMT.Utilitários.JWT;
using ControleEPI.DTO.E_Mail;

namespace ApiSMT
{
    /// <summary>
    /// Classe Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Construtor Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Interface de configuração
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Função que configura os serviços do sistema
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddTransient<ITokenService, TokenService>();

            string SMTConnection = Configuration.GetConnectionString("smt");
            string RHConnection = Configuration.GetConnectionString("rh");
            
            services.AddDbContextPool<AppDbContext>(options => options.UseMySql(SMTConnection, ServerVersion.AutoDetect(SMTConnection)));
            services.AddDbContextPool<AppDbContextRH>(options => options.UseMySql(RHConnection, ServerVersion.AutoDetect(RHConnection)));
            services.AddDbContextPool<VestAppDbContext>(options => options.UseMySql(SMTConnection, ServerVersion.AutoDetect(SMTConnection)));

            services.Configure<EmailSettingsDTO>(Configuration.GetSection("EmailSettings"));

            //EPI
            services.AddScoped<ICategoriasBLL, CategoriaDAL>();
            services.AddScoped<IConUserBLL, ConUserDAL>();
            services.AddScoped<IEpiVinculoBLL, EpiVinculoDAL>();
            services.AddScoped<IMotivosBLL, MotivosDAL>();
            services.AddScoped<IPedidosBLL, PedidosDAL>();
            services.AddScoped<IPedidosStatusBLL, PedidosStatusDAL>();
            services.AddScoped<IProdutosBLL, ProdutosDAL>();
            services.AddScoped<IStatusBLL, StatusDAL>();
            services.AddScoped<IFornecedoresBLL, FornecedorDAL>();
            services.AddScoped<IEpiVinculoBLL, EpiVinculoDAL>();
            services.AddScoped<ILogEstoqueBLL, LogEstoqueDAL>();
            services.AddScoped<IEmpContratosBLL, EmpContratosDAL>();
            services.AddScoped<ICargosBLL, CargosDAL>();
            services.AddScoped<IDepartamentosBLL, DepartamentosDAL>();
            services.AddScoped<IComprasBLL, ComprasDAL>();
            services.AddScoped<IItensBLL, ItensDAL>();
            services.AddTransient<IMailServiceBLL, MailService>();

            //Vestimenta
            services.AddScoped<IVestimentaBLL, VestimentaDAL>();
            services.AddScoped<IComprasVestBLL, ComprasVestDAL>();
            services.AddScoped<IStatusVestBLL, StatusVestDAL>();
            services.AddScoped<IPedidosVestBLL, PedidosVestDAL>();
            services.AddScoped<IEstoqueBLL, EstoqueDAL>();
            services.AddScoped<ILogBLL, LogDAL>();
            services.AddScoped<IVestVinculoBLL, VestVinculoDAL>();
            services.AddScoped<IVestRepositorioBLL, VestRepositorioDAL>();
            services.AddScoped<IVestItemVinculoBLL, VestItemVinculoDAL>();

            services.AddControllers();
            services.AddHostedService<TimerHostedService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AbertoAtodos", builder => builder                
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiSMT", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below. \r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Funçao de configuraçao de acessos a api e documentação
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiSMT v1"));
            }
            else
            {
                //app.UseHttpsRedirection();
            }

            app.UseAuthentication();

            app.UseRouting();
            app.UseCors("AbertoAtodos");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllers();
            });
        }
    }
}
