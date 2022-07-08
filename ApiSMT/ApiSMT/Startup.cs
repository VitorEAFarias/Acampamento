using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ControleEPI.DTO._DbContext;
using ControleEPI.DAL;
using ControleEPI.BLL;
using Microsoft.EntityFrameworkCore;
using ApiSMT.Utilit�rios;
using System.Reflection;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        /// Interface de configura��o
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Fun��o que configura os servi�os do sistema
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication
                 (JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,

                         ValidIssuer = Configuration["Jwt:Issuer"],
                         ValidAudience = Configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey
                       (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                     };
                 });

            string SMTConnection = Configuration.GetConnectionString("smt");
            string RHConnection = Configuration.GetConnectionString("rh");
            
            services.AddDbContextPool<AppDbContext>(options => options.UseMySql(SMTConnection, ServerVersion.AutoDetect(SMTConnection)));
            services.AddDbContextPool<AppDbContextRH>(options => options.UseMySql(RHConnection, ServerVersion.AutoDetect(RHConnection)));

            //EPI
            services.AddScoped<ICategoriasDAL, CategoriaBLL>();
            services.AddScoped<IConUserDAL, ConUserBLL>();
            services.AddScoped<IEpiVinculoDAL, EpiVinculoBLL>();
            services.AddScoped<IMotivosDAL, MotivosBLL>();
            services.AddScoped<IPedidosDAL, PedidosBLL>();
            services.AddScoped<IPedidosStatusDAL, PedidosStatusBLL>();
            services.AddScoped<IProdutosDAL, ProdutosBLL>();
            services.AddScoped<IStatusDAL, StatusBLL>();
            services.AddScoped<IFornecedoresDAL, FornecedorBLL>();
            services.AddScoped<IEpiVinculoDAL, EpiVinculoBLL>();
            services.AddScoped<ILogEstoqueDAL, LogEstoqueBLL>();
            services.AddScoped<IEmpContratosDAL, EmpContratosBLL>();
            services.AddScoped<ICargosDAL, CargosBLL>();
            services.AddScoped<IDepartamentosDAL, DepartamentosBLL>();
            //---

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
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
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
        /// Fun�ao de configura�ao de acessos a api e documenta��o
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

            //app.UseHttpsRedirection();
            
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
