using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Interface
{
    public interface ISolicitudSMSMasivoNumeroRepository
    {
        public Task<bool> Registrar(Guid solicitudId, List<string> telefonos);
        Task<List<string>> ListarTelefonosEnviados(Guid solicitudId);

    }
}
