using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Interface
{
    public interface ISolicitudSMSMasivoRepository
    {
        Task<SolicitudSMSMasivoResponse> Obtener(Guid SolicitudId);
        Task<bool> ActualizarEstado(Guid solicitudId,int Estado);
        Task<bool> ActualizarResumenGeneral(Guid solicitudId, int totalProspectos, int celularesValidos, int totalEnviados);
    }
}
