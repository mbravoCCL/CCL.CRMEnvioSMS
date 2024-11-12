using CCL.CRMEnvioSMS.Core.Interface;
using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Data.Repository;
using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using CCL.CRMEnvioSMS.Utility.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CCL.CRMEnvioSMS.Utility.Constantes.Constantes;

namespace CCL.CRMEnvioSMS.Core.Service
{
    public class CampaignSolicitudSMSService : ICampaignSolicitudSMSService
    {
        private readonly ISMSService _smsService;
        private readonly IFichaInscripcionRepository _fichaInscripcionRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly ISolicitudSMSMasivoRepository _solicitudSMSMasivoRepository;
        private readonly ICampaignSolicitudSMSRepository _campaignSolicitudSMSRepository;
        public CampaignSolicitudSMSService(ISMSService smsService,
                                         IFichaInscripcionRepository fichaInscripcionRepository,
                                         ISolicitudSMSMasivoRepository solicitudSMSMasivoRepository,
                                         IEventoRepository eventoRepository,
                                         ICampaignSolicitudSMSRepository campaignSolicitudSMSRepository,
                                         ICampaingSMSNumerosRepository campaingSMSNumerosRepository)
        {
            _smsService = smsService;
            _fichaInscripcionRepository = fichaInscripcionRepository;
            _eventoRepository = eventoRepository;
            _solicitudSMSMasivoRepository = solicitudSMSMasivoRepository;
            _campaignSolicitudSMSRepository = campaignSolicitudSMSRepository;
        }

        public async Task<DetalleEnvioSMSResponse> detalleEnvioSMSService(Guid solicitudId)
        {
            var solicitudMasivo = await _solicitudSMSMasivoRepository.Obtener(solicitudId);
            var idEvento = solicitudMasivo.new_evento;
            var evento = await _eventoRepository.Obtener(idEvento);

            if (solicitudMasivo.new_estado != Constantes.EstadoSolicitudSMSMasivo.ENVIADO)
            {
                return new DetalleEnvioSMSResponse
                {
                    estadoSolicitud = Constantes.EstadoSolicitudSMSMasivo.GetEstadoText(solicitudMasivo.new_estado),
                    evento = evento.new_name,
                    solicitud = solicitudMasivo.new_name,
                    destinatarios = new List<Destinatarios>()
                };
            }

            var campaing = await _campaignSolicitudSMSRepository.ListarIdsPorSolicitud(solicitudId);

            var reportCampaingResponses = new List<ReportCampaingResponse>();
            var fichaInscripcion = await _fichaInscripcionRepository.FichaInscripcion(idEvento);

            //POR SERVICO DE ENVIA MAS
            foreach (var itemCampaing in campaing)
            {
                try
                {
                   
                    var reportCampaing = await _smsService.ReportCampaign(itemCampaing);
                    if (reportCampaing?.data?.sends?.Any() == true)
                    {
                        reportCampaingResponses.Add(reportCampaing);
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            var destinatarios = new List<Destinatarios>();
            foreach (var reportCampaing in reportCampaingResponses)
            {
                foreach (var send in reportCampaing.data.sends)
                {
                    var ficha = fichaInscripcion.FirstOrDefault(f =>
                        f.new_TelefonoParticular == send.phone ||
                        f.new_TelefonoLaboral == send.phone ||
                        f.new_contactonextelrpm == send.phone ||
                        f.new_CTelefono04 == send.phone);


                        var destinatario = new Destinatarios
                        {
                            nombreCompleto = ficha?.fullname ?? "No especificado.",
                            telefono = send.phone,
                            status = ConvertirStatusAEspanol(send.status),
                            enviado = send.send_at,
                            carrier = send.carrier,
                            sms = send.text
                        };

                        destinatarios.Add(destinatario);

                }
            }

            return new DetalleEnvioSMSResponse
            {
                estadoSolicitud = Constantes.EstadoSolicitudSMSMasivo.GetEstadoText(solicitudMasivo.new_estado),
                evento = evento.new_name,
                solicitud = solicitudMasivo.new_name,
                destinatarios = destinatarios.OrderByDescending(d => d.status).ToList()
            };
        }

        public async Task<DetalleEnvioSMSResponse> detalleEnvioSMSBD(Guid solicitudId)
        {
            var solicitudMasivo = await _solicitudSMSMasivoRepository.Obtener(solicitudId);
            var idEvento = solicitudMasivo.new_evento;
            var evento = await _eventoRepository.Obtener(idEvento);

            if (solicitudMasivo.new_estado != Constantes.EstadoSolicitudSMSMasivo.ENVIADO)
            {
                return new DetalleEnvioSMSResponse
                {
                    estadoSolicitud = Constantes.EstadoSolicitudSMSMasivo.GetEstadoText(solicitudMasivo.new_estado),
                    evento = evento.new_name,
                    solicitud = solicitudMasivo.new_name,
                    destinatarios = new List<Destinatarios>()
                };
            }



            var campaing = await _campaignSolicitudSMSRepository.ListarIdsPorSolicitud(solicitudId);

            var fichaInscripcion = await _fichaInscripcionRepository.FichaInscripcion(idEvento);

            //POR BASE DE DATOS
            var dataSend = new List<Send>();
            if (campaing != null && campaing.Any())
            {
                string campaignIds = string.Join(",", campaing);
                dataSend = await _campaignSolicitudSMSRepository.ListarCampaingSolicitudDetalle(campaignIds);
            }

            var destinatarios = new List<Destinatarios>();

           foreach (var send in dataSend)
                {
                    var ficha = fichaInscripcion.FirstOrDefault(f =>
                        f.new_TelefonoParticular == send.phone ||
                        f.new_TelefonoLaboral == send.phone ||
                        f.new_contactonextelrpm == send.phone ||
                        f.new_CTelefono04 == send.phone);


                    var destinatario = new Destinatarios
                    {
                        nombreCompleto = ficha?.fullname ?? "No especificado.",
                        telefono = send.phone,
                        status = ConvertirStatusAEspanol(send.status),
                        enviado = send.send_at,
                        carrier = send.carrier,
                        sms = send.text
                    };

                    destinatarios.Add(destinatario);

            }
            

            return new DetalleEnvioSMSResponse
            {
                estadoSolicitud = Constantes.EstadoSolicitudSMSMasivo.GetEstadoText(solicitudMasivo.new_estado),
                evento = evento.new_name,
                solicitud = solicitudMasivo.new_name,
                destinatarios = destinatarios.OrderByDescending(d => d.status).ToList()
            };
        }

        public static string ConvertirStatusAEspanol(string status)
        {
            return status switch
            {
                EnviaMasReportCampaignStatus.DELIVERED => ReportCampaignStatus.ENTREGADO,
                EnviaMasReportCampaignStatus.REJECTED => ReportCampaignStatus.RECHAZADO,
                _ => status 
            };
        }
    }
}
