using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Core.Interface
{
    public interface ISolicitudSMSMasivoService
    {
        public Task<SolicitudSMSMasivoResponse> Obtener(Guid SolicitudId);
        public Task<SmsMasivoResponse> smsMasivo(Guid idSolicitud);
    }
}
