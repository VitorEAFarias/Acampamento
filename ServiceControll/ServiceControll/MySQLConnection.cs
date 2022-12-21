using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ServiceControll
{
    public class MySQLConnection
    {
        public MySqlConnection Con;
        public MySqlCommand Cmd;
        public MySqlDataReader Dr;

        LogEvent _log = new LogEvent();

        public void OpenConnection()
        {
            try
            {
                //string conMySqlTeste = "Server=192.168.0.89;Database=tarifador_srv;Uid=reis;Pwd=Reis2015;Pooling=True;SslMode=None;default command timeout=280";
                string conMySql = "Server=10.0.3.41;Database=tarifador_srv;Uid=rocontrole;Pwd=Reis@2021;Pooling=True;SslMode=None;default command timeout=280";
                Con = new MySqlConnection(conMySql);
                Con.Open();
                _log.WriteEntry("Conexão realizada com sucesso : " + EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                _log.WriteEntry("Erro ao realizar a conexão com MySql : " + ex.Message.ToString(), EventLogEntryType.Error);
                throw new Exception(ex.Message);
            }
        }
        public void CloseConnection()
        {
            try
            {
                Con.Dispose();
                Con.Close();
            }
            catch (Exception ex)
            {

                _log.WriteEntry("Erro de Conexao: " + ex.Message.ToString(), EventLogEntryType.Error);
            }

        }
    }
}
