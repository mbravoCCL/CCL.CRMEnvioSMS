using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Response
{
    public class ReportCampaingResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public List<Send>? sends { get; set; }
    }

    public class Send
    {
        public string message_id { get; set; }
        public string phone { get; set; }
        public string text { get; set; }
        public string send_at { get; set; }
        public string status { get; set; }
        public string carrier { get; set; }
    }
}
