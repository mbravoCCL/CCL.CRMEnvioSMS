using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Interface
{
    public interface ICampaignSolicitudSMSRepository
    {
        public Task<bool> Registrar(String campaignId,Guid solicitudId, int totalNumeros);
        Task<List<string>> ListarIdsPorSolicitud(Guid solicitudId);
    }
}
