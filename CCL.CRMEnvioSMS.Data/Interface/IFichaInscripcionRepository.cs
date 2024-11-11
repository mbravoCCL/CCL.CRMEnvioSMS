using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Interface
{
    public interface IFichaInscripcionRepository
    {
        Task<List<FichaInscripcionTelefonoResponse>> ListarTelefonos(FichaInscripcionTelefonoRequest fichaInscripcionTelefonoRequest);
        Task<int> CantidadPorEvento(Guid EventoId);
        Task<List<FichaInscripcionResponse>> FichaInscripcion(Guid eventoId);
    }
}
