using CCL.CRMEnvioSMS.Core.Interface;
using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Core.Service
{
    public class FichaInscripcionService : IFichaInscripcionService
    {
        private readonly IFichaInscripcionRepository _repository;

        public FichaInscripcionService(IFichaInscripcionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FichaInscripcionTelefonoResponse>> ListarTelefonos(FichaInscripcionTelefonoRequest fichaInscripcionTelefonoRequest)
        {
            return await _repository.ListarTelefonos(fichaInscripcionTelefonoRequest);
        }
    }
}
