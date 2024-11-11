using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Interface
{
    public interface IEventoRepository
    {
        Task<EventoResponse> Obtener(Guid EventoId);
    }
}
