using CCL.CRMEnvioSMS.Core.Interface;
using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Data.Repository;
using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using CCL.CRMEnvioSMS.Utility.Constantes;
using CCL.CRMEnvioSMS.Utility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CCL.CRMEnvioSMS.Utility.Constantes.Constantes;

namespace CCL.CRMEnvioSMS.Core.Service
{
    public class SolicitudSMSMasivoService : ISolicitudSMSMasivoService
    {
        private readonly ISMSService _smsService;
        private readonly IFichaInscripcionRepository _fichaInscripcionRepository;
        private readonly ISolicitudSMSMasivoRepository _solicitudSMSMasivoRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly ISolicitudSMSMasivoNumeroRepository _solicitudSMSMasivoNumeroRepository;
        private readonly ICampaignSolicitudSMSRepository _campaignSolicitudSMSRepository;
        private readonly ICampaingSMSNumerosRepository _campaingSMSNumerosRepository;
        public SolicitudSMSMasivoService(ISMSService smsService,
                                         IFichaInscripcionRepository fichaInscripcionRepository,
                                         ISolicitudSMSMasivoRepository solicitudSMSMasivoRepository,
                                         IEventoRepository eventoRepository,
                                         ISolicitudSMSMasivoNumeroRepository solicitudSMSMasivoNumeroRepository,
                                         ICampaignSolicitudSMSRepository campaignSolicitudSMSRepository,
                                         ICampaingSMSNumerosRepository campaingSMSNumerosRepository)
        {
            _smsService = smsService;
            _fichaInscripcionRepository = fichaInscripcionRepository;
            _solicitudSMSMasivoRepository = solicitudSMSMasivoRepository;
            _eventoRepository = eventoRepository;
            _solicitudSMSMasivoNumeroRepository = solicitudSMSMasivoNumeroRepository;
            _campaignSolicitudSMSRepository = campaignSolicitudSMSRepository;
            _campaingSMSNumerosRepository = campaingSMSNumerosRepository;
        }
 
        public async Task<SolicitudSMSMasivoResponse> Obtener(Guid SolicitudId)
        {
           return await _solicitudSMSMasivoRepository.Obtener(SolicitudId);   
        }

        public async Task<SmsMasivoResponse> smsMasivo(Guid idSolicitud)
        {
            await Task.Delay(3000);
            var solicitudMasivo = await _solicitudSMSMasivoRepository.Obtener(idSolicitud);
            var evento = await _eventoRepository.Obtener(solicitudMasivo.new_evento);
            var fechaActual = DateTime.Now.Date;

            if (evento.new_HorarioFin == null)
            {
                throw new Exception("La fecha de finalización del evento es nula.");
            }

            if (fechaActual > evento.new_HorarioFin.Date)
            {
                throw new Exception($"El evento {evento.new_name} ya ha finalizado. Fecha Fin: {evento.new_HorarioFin:dd/MM/yyyy}");

            }

            if (solicitudMasivo.new_estado == Constantes.EstadoSolicitudSMSMasivo.EN_PROCESO)
            {
                throw new Exception($"La solicitud está actualmente en proceso de envío. No se puede iniciar un nuevo envío masivo hasta que este proceso finalice. Esto puede tardar algunos minutos, especialmente si hay muchos destinatarios.");
            }
            else if (solicitudMasivo.new_estado != Constantes.EstadoSolicitudSMSMasivo.PENDIENTE_ENVIO &&
                     solicitudMasivo.new_estado != Constantes.EstadoSolicitudSMSMasivo.ENVIO_ADICIONAL)
            {
                throw new Exception($"La solicitud debe estar en estado Pendiente Envío o Envío Adicional para el envío de SMS. Estado Actual: {EstadoSolicitudSMSMasivo.GetEstadoText(solicitudMasivo.new_estado)}");
            }

            await _solicitudSMSMasivoRepository.ActualizarEstado(idSolicitud,Constantes.EstadoSolicitudSMSMasivo.EN_PROCESO);
            var fichaInscripcion = await _fichaInscripcionRepository.ListarTelefonos(new FichaInscripcionTelefonoRequest
                                                                                    {
                                                                                        EventoId = solicitudMasivo.new_evento,
                                                                                        SolicitudId = idSolicitud
                                                                                    });

            var fichaInscripcionNoEnviados = await FiltrarTelefonosNoEnviados(idSolicitud,fichaInscripcion);
            var request = GetCampaignRequest(solicitudMasivo, fichaInscripcionNoEnviados);

            if (!request.messages.Any())
            {
                await _solicitudSMSMasivoRepository.ActualizarEstado(idSolicitud, Constantes.EstadoSolicitudSMSMasivo.ENVIADO);
                throw new Exception("No hay destinatarios disponibles para el envío del SMS, o ya se les ha enviado previamente.");
            }

            var _smsResponse = await _smsService.CreateCampaign(request);
            var telefonosEnviados = ObtenerTelefonosEnviados(solicitudMasivo, fichaInscripcionNoEnviados);
           // await ActualizarResumenGeneral(idSolicitud, solicitudMasivo.new_evento, telefonosEnviados);
            await _solicitudSMSMasivoNumeroRepository.Registrar(idSolicitud, telefonosEnviados);

            if (!string.IsNullOrEmpty(solicitudMasivo.new_incluirmovil01))
            {
                telefonosEnviados.Add(solicitudMasivo.new_incluirmovil01);
            }

            if (!string.IsNullOrEmpty(solicitudMasivo.new_incluirmovil02))
            {
                telefonosEnviados.Add(solicitudMasivo.new_incluirmovil02);
            }


            await _campaignSolicitudSMSRepository.Registrar(_smsResponse.Data.campaign_id, idSolicitud, telefonosEnviados.Count);
            await _campaingSMSNumerosRepository.Registrar(_smsResponse.Data.campaign_id, telefonosEnviados);


            return new SmsMasivoResponse
            {
                Campaign = _smsResponse,
                Telefonos = telefonosEnviados
            };
        }

        private async Task<bool> ActualizarResumenGeneral(Guid solicitudId, Guid eventoId,  List<String> telefonosEnviados)
        {
            var cantidad = await _fichaInscripcionRepository.CantidadPorEvento(eventoId);
            var response = await _solicitudSMSMasivoRepository.ActualizarResumenGeneral(solicitudId,
                                                                                        cantidad,
                                                                                        ValidarTelefonos(telefonosEnviados),
                                                                                        telefonosEnviados.Count);

            return response;
        }

        public async Task<List<FichaInscripcionTelefonoResponse>> FiltrarTelefonosNoEnviados(Guid solicitudId,
                                                                                List<FichaInscripcionTelefonoResponse> telefonos)
        {
            var telefonosNoEnviados = new List<FichaInscripcionTelefonoResponse>();
            var telefonosEnviados = await _solicitudSMSMasivoNumeroRepository.ListarTelefonosEnviados(solicitudId);

            foreach (var item in telefonos)
            {
                var telefonoValido = item.new_TelefonoParticular ??
                                           item.new_TelefonoLaboral ??
                                           item.new_contactonextelrpm ??
                                           item.new_CTelefono04;


                if (!string.IsNullOrEmpty(telefonoValido) && !telefonosEnviados.Contains(telefonoValido))
                {
                    telefonosNoEnviados.Add(item);
                }
            }

            return telefonosNoEnviados;
        }
        private List<string> ObtenerTelefonosEnviados(SolicitudSMSMasivoResponse solicitudMasivo, List<FichaInscripcionTelefonoResponse> fichaInscripcion)
        {
            var telefonosEnviados = fichaInscripcion
                .Select(f => f.new_TelefonoParticular ?? f.new_TelefonoLaboral ?? f.new_contactonextelrpm ?? f.new_CTelefono04 ?? "")
                .ToList();

            return telefonosEnviados;
        }


        public int ValidarTelefonos(List<string> telefonosEnviados)
        {
            int cantidadValidos = 0;

            foreach (var telefono in telefonosEnviados)
            {
                if (!string.IsNullOrEmpty(telefono) &&
                    telefono.Length == 9 &&  
                    telefono.StartsWith("9") &&
                    telefono.All(char.IsDigit)) 
                {
                    cantidadValidos++;
                }
            }

            return cantidadValidos; 
        }


        private CampaignRequest GetCampaignRequest(SolicitudSMSMasivoResponse solicitudMasivo, List<FichaInscripcionTelefonoResponse> fichaInscripcionTelefonoResponses)
        {
            return new CampaignRequest
            {
                campaign_name = solicitudMasivo.new_name ?? "CCL",
                messages = GetMessages(fichaInscripcionTelefonoResponses, solicitudMasivo),
                options = new Option
                {
                    push = false,
                    is_bidireccional = false
                }
            };
        }

        private List<Message> GetMessages(List<FichaInscripcionTelefonoResponse> fichaInscripcionTelefonoResponses, SolicitudSMSMasivoResponse solicitudMasivo)
        {
            List<Message> messages = new List<Message>();

          
            foreach (var item in fichaInscripcionTelefonoResponses)
            {
               var telefono = item.new_TelefonoParticular ??
               item.new_TelefonoLaboral ??
               item.new_contactonextelrpm ??
               item.new_CTelefono04;

                if (!string.IsNullOrEmpty(telefono)) 
                {
                    messages.Add(CreateMessage(solicitudMasivo.new_mensaje, telefono));
                }
            }

            if (!string.IsNullOrEmpty(solicitudMasivo.new_incluirmovil01))
            {
                messages.Add(CreateMessage(solicitudMasivo.new_mensaje, solicitudMasivo.new_incluirmovil01));
            }

            if (!string.IsNullOrEmpty(solicitudMasivo.new_incluirmovil02))
            {
                messages.Add(CreateMessage(solicitudMasivo.new_mensaje, solicitudMasivo.new_incluirmovil02));
            }

            return messages;
        }

        private Message CreateMessage(string mensaje, string phone)
        {
            return new Message
            {
                text = mensaje ?? "",  
                message_id = $"id-{Guid.NewGuid()}",
                phone = phone  
            };
        }
    }
}
