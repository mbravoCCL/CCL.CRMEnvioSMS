using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Response
{
    public class CampaignResponse
    {
        public bool Succcess { get; set; }
        public string Message { get; set; }
        public DataCampaign Data { get; set; }
    }

    public class DataCampaign
    {
        public string campaign_id { get; set; }
    }
}
