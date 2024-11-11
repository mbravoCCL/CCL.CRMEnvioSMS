using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Core.Interface
{
    public interface IFichaInscripcionService
    {
        Task<List<FichaInscripcionTelefonoResponse>> ListarTelefonos(FichaInscripcionTelefonoRequest fichaInscripcionTelefonoRequest);
    }
}
