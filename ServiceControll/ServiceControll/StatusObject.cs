using System;

namespace ServiceControll
{
    public class StatusObject
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string status { get; set; }
        public string serviceName { get; set; }
        public DateTime timeServiceCheck { get; set; }
        public int arquivosLeitura { get; set; }        
    }
}
