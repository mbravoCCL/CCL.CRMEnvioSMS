using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Response
{
    public class EventoResponse
    {
        public Guid new_eventoId { get; set; }
        public string new_name { get; set; }
        public DateTime new_HorarioFin { get; set; }
    }
}
