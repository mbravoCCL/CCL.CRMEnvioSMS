using CCL.CRMEnvioSMS.Entity.Models.Response;
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
        Task<List<Send>> ListarCampaingSolicitudDetalle(string campaign_ids);
    }
}
