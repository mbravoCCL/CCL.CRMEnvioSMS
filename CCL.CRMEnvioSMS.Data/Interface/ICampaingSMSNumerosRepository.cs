using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Interface
{
    public interface ICampaingSMSNumerosRepository
    {
         Task<bool> Registrar(string campaign_id, List<string> telefonos);
    }
}
