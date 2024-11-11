using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Request
{
    public class FichaInscripcionTelefonoRequest
    {
        public Guid EventoId { get; set; }
        public Guid SolicitudId { get; set; }
    }
}
