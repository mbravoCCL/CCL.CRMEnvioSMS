using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Core.Interface
{
    public interface ISMSService
    {
        public Task<CampaignResponse> CreateCampaign(CampaignRequest compaignMessage);
        Task<ReportCampaingResponse> ReportCampaign(String campaingId);
    }
}
