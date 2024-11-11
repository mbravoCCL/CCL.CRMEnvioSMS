using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Response
{
    public class SmsMasivoResponse
    {
        public CampaignResponse Campaign { get; set; }
        public List<string> Telefonos { get; set; }
    }
}
