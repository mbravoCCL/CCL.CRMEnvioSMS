using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Entity.Models.Request
{
    public class CampaignRequest
    {
        public string campaign_name { get; set; }
        public List<Message> messages { get; set; }
        public Option options { get; set; }
    }

    public class Message
    {
        public string phone { get; set; }
        public string text { get; set; }
        public string message_id { get; set; }
    }

    public class Option
    {
        public bool push { get; set; }
        public bool is_bidireccional { get; set; }
    }
}
