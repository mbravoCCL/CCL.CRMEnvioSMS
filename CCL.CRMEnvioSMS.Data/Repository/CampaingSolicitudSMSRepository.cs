using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CCL.CRMEnvioSMS.Data.Repository
{
    public class CampaingSolicitudSMSRepository : ICampaignSolicitudSMSRepository
    {
        private readonly string conexionSQL;

        public CampaingSolicitudSMSRepository(Settings connection)
        {
            conexionSQL = connection.getConnectionEnviaMasSMS_CRM();
        }

        public async Task<bool> Registrar(string campaignId, Guid solicitudId, int totalNumeros)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();

                try
                {
                    var resultado = await connection.ExecuteAsync(
                        "dbo.sp_InsertarCampaingSolicitudSMS",
                        new { campaign_id = campaignId, new_solicituddesmsmasivoId = solicitudId , totalnumeros = totalNumeros },
                        commandType: CommandType.StoredProcedure
                    );

                    return resultado > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al registrar el estado de la solicitud SMS masivo", ex);
                }
            }
        }

        public async Task<List<string>> ListarIdsPorSolicitud(Guid solicitudId)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();
                try
                {
                    var response = await connection.QueryAsync<string>(
                        "dbo.sp_ListarCampaignIdPorSolicitud",
                        new { SolicitudId = solicitudId },
                        commandType: CommandType.StoredProcedure);

                    return response?.ToList() ?? new List<string>(); ;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sp_ListarCampaignIdPorSolicitud", ex);
                }
            }
        }
    }
}
