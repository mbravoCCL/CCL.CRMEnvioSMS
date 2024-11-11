using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using CCL.CRMEnvioSMS.Utility;


namespace CCL.CRMEnvioSMS.Data.Repository
{
    public class SolicitudSMSMasivoRepository : ISolicitudSMSMasivoRepository
    {
        private readonly string conexionSQL;

        public SolicitudSMSMasivoRepository(Settings connection)
        {
            conexionSQL = connection.getConnectionCRMProd();
        }

        public async Task<SolicitudSMSMasivoResponse> Obtener(Guid solicitudId)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();
                try
                {
                    var response = await connection.QuerySingleAsync<SolicitudSMSMasivoResponse>(
                                     "Sistema.sp_SolicitudSMSMasivoPorId",
                                     new { SolicitudId = solicitudId },
                                     commandType: CommandType.StoredProcedure);

                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sp_SolicitudSMSMasivoPorId", ex);
                }
            }
        }


        public async Task<bool> ActualizarEstado(Guid solicitudId, int nuevoEstado)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();

                try
                {
                    var resultado = await connection.ExecuteAsync(
                        "Sistema.sp_ActualizarEstadoSolicitudSMSMasivo", 
                        new { SolicitudId = solicitudId, NuevoEstado = nuevoEstado }, 
                        commandType: CommandType.StoredProcedure
                    );

                    return resultado > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el estado de la solicitud SMS masivo", ex);
                }
            }
        }


        public async Task<bool> ActualizarResumenGeneral(Guid solicitudId, int totalProspectos, int celularesValidos, int totalEnviados)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();

                try
                {
                    var resultado = await connection.ExecuteAsync(
                        "Sistema.sp_ActualizarResumenGeneralSolicitudSMSMasivo",
                        new { SolicitudId = solicitudId, TotalProspectos = totalProspectos,
                              CelularesValidos = celularesValidos , TotalEnviados = totalEnviados 
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    return resultado > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar sp_ActualizarResumenGeneralSolicitudSMSMasivo", ex);
                }
            }
        }
    }
}
