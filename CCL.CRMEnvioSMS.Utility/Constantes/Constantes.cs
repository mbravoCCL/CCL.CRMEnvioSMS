using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Utility.Constantes
{
    public static class Constantes
    {
        public static class EstadoSolicitudSMSMasivo
        {
            public const int ENVIADO = 100000002;
            public const int PENDIENTE_ENVIO = 100000001;
            public const int ENVIO_ADICIONAL = 100000003;
            public const int EN_PROCESO = 100000004;

            private static readonly Dictionary<int, string> _estadoMap = new Dictionary<int, string>
            {
                { ENVIADO, "ENVIADO" },
                { PENDIENTE_ENVIO, "PENDIENTE ENVIO" },
                { ENVIO_ADICIONAL, "ENVIO ADICIONAL" },
                { EN_PROCESO, "EN PROCESO" }
            };

            public static string GetEstadoText(int estadoCodigo)
            {
                return _estadoMap.TryGetValue(estadoCodigo, out var estadoText) ? estadoText : "Estado desconocido";
            }
        }

        public static class EnviaMasReportCampaignStatus
        {
            public const string DELIVERED = "DELIVERED";
            public const string REJECTED = "REJECTED";
        }

        public static class ReportCampaignStatus
        {
            public const string ENTREGADO = "ENTREGADO";
            public const string RECHAZADO = "RECHAZADO";
        }


    }
}
