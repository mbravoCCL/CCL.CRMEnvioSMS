using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Response
{
    public class DetalleEnvioSMSResponse
    {
        public string evento { get; set; }
        public string solicitud { get; set; }
        public string estadoSolicitud { get; set; }
        public List<Destinatarios> destinatarios {get;set;}
        
    }

    public class Destinatarios
    {
        public string? nombreCompleto { get; set; }
        public string? telefono { get; set; }
        public string? status { get; set; }
        public string? enviado { get; set; }
        public string? carrier { get; set; }
        public string? sms { get; set; }
    }
}
