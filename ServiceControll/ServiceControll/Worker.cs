using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceControll
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("A verificação foi iniciada.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Executando Serviço: {time}", DateTimeOffset.Now);

                bool isAdmin;
                try
                {
                    WindowsIdentity user = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(user);
                    isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

                    _logger.LogInformation("Administrador!.");
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new Exception("Acesso como admin negado" + Environment.NewLine + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possível conceder acesso como Admin" + Environment.NewLine + ex.Message);
                }                

                ServiceController[] services = ServiceController.GetServices();

                MySQLConnection conectar = new MySQLConnection();

                

                StatusObject objeto = new StatusObject();
                //C:\Projects\dotnet\RO.ServiceStatus\ROServiceCheck\bin\Release\net5.0  
                //C:\Temp\ArquivosLeitura_Api5
                objeto.arquivosLeitura = Directory.GetFiles(@"C:\Temp\ArquivosLeitura_Api5", "*.*", SearchOption.TopDirectoryOnly).Count();

                foreach (ServiceController service in services)
                {
                    try
                    {
                        //Ro.Upload.Analisys
                        if (service.ServiceName.Equals("Ro.Upload.Analisys"))
                        {
                            conectar.OpenConnection();
                            //var nomeServico = service.ServiceName.ToString();
                            var StatusServico = service.Status.ToString();

                            if (StatusServico.Equals("Running"))
                            {
                                objeto.status = "Rodando";
                                objeto.serviceName = service.ServiceName;
                                _logger.LogInformation("Serviço rodando : " + service.ServiceName.ToString());
                            }
                            else
                            {
                                objeto.status = "Parado";
                                objeto.serviceName = service.ServiceName;
                                _logger.LogInformation("Serviço parado : " + service.ServiceName.ToString());
                            }                            

                            string Sql = "Update `tarifador_srv`.`servicestatus` SET";
                            Sql += " status=@v1,";
                            Sql += " timeServiceCheck=@v2,";
                            Sql += " arquivosLeitura=@v3";
                            Sql += " where serviceName='" + service.ServiceName + "'";

                            using (conectar.Cmd = new MySqlCommand(Sql, conectar.Con))
                            {
                                conectar.Cmd.CommandText = Sql;
                                conectar.Cmd.Parameters.AddWithValue("@v3", objeto.arquivosLeitura);
                                conectar.Cmd.Parameters.AddWithValue("@v2", DateTime.Now);
                                conectar.Cmd.Parameters.AddWithValue("@v1", objeto.status);

                                conectar.Cmd.ExecuteNonQuery();
                            }

                            conectar.CloseConnection();
                        }
                        else if (service.ServiceName.Equals("Api5.RoControle"))
                        {
                            conectar.OpenConnection();

                            string Sql = "Select `arquivosLeitura`";
                            Sql += "From `tarifador_srv`.`servicestatus`";

                            using (conectar.Cmd = new MySqlCommand(Sql, conectar.Con))
                            {
                                conectar.Cmd.CommandText = Sql;
                                MySqlDataReader result = conectar.Cmd.ExecuteReader();

                                if (objeto.arquivosLeitura >= 300)
                                {
                                    if (result.GetInt32(result.GetOrdinal("arquivosLeitura")) <= objeto.arquivosLeitura)
                                    {
                                        service.Refresh();

                                        var StatusServico = service.Status.ToString();

                                        if (StatusServico.Equals("Running"))
                                        {
                                            service.Stop();
                                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                                            service.Start();
                                        }
                                        else if (StatusServico.Equals("Stopped"))
                                        {
                                            service.Start();
                                            service.WaitForStatus(ServiceControllerStatus.Running);
                                        }
                                    }
                                }
                            }

                            conectar.CloseConnection();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                await Task.Delay(1800000, stoppingToken);
            }
        }
    }
}
