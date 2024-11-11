using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Response
{
    public class SolicitudSMSMasivoResponse
    {
        public string? new_name { get; set; }
        public string? new_mensaje { get; set; }
        public string? new_incluirmovil01 { get; set; }
        public string? new_incluirmovil02 { get; set; }
        public Guid new_evento { get; set; }
        public int new_estado { get; set; }
    }
}
