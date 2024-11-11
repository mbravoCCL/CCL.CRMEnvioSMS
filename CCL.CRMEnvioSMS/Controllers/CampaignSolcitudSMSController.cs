using CCL.CRMEnvioSMS.Core.Interface;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCL.CRMEnvioSMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignSolcitudSMSController :ControllerBase
    {
        private readonly ICampaignSolicitudSMSService _service;
        public CampaignSolcitudSMSController(ICampaignSolicitudSMSService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("detalle/{idSolicitud}")]
        public async Task<DetalleEnvioSMSResponse> detalleEnvioSMS(Guid idSolicitud)
        {
            return await _service.detalleEnvioSMS(idSolicitud);
        }
    }
}
