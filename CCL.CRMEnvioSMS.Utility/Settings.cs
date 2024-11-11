using CCL.CRMEnvioSMS.Utility.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Utility
{
    public class Settings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string getConnectionCRMProd()
        {
            return _configuration.GetConnectionString("CamaraCrmPROD") ?? "";
        }

        public string getConnectionEnviaMasSMS_CRM()
        {
            return _configuration.GetConnectionString("EnviaMasSMS_CRM") ?? "";
        }

        public EnviaMasSettings GetEnviaMasSettings()
        {
            var enviaMasSettings = _configuration.GetSection("EnviaMas") as EnviaMasSettings;
            return enviaMasSettings;
        }
    }
}
