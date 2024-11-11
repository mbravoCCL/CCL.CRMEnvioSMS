using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Entity.Models.Request;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using CCL.CRMEnvioSMS.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Repository
{
    public class FichaInscripcionRepository : IFichaInscripcionRepository
    {
        private readonly string conexionSQL;

        public FichaInscripcionRepository(Settings connection)
        {
            conexionSQL = connection.getConnectionCRMProd();
        }

        public async Task<List<FichaInscripcionTelefonoResponse>> ListarTelefonos(FichaInscripcionTelefonoRequest request)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();
                try
                {
                    var response = await connection.QueryAsync<FichaInscripcionTelefonoResponse>(
                        "Sistema.sp_FichaInscripcionTelefonosPorEstadoAtencion",
                        new { EventoId = request.EventoId, SolicitudId = request.SolicitudId },
                        commandType: CommandType.StoredProcedure);

                    return response?.ToList() ?? new List<FichaInscripcionTelefonoResponse>(); ;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sp_FichaInscripcionTelefonosPorEstadoAtencion", ex); 
                }
            } 
        }

        public async Task<List<FichaInscripcionResponse>> FichaInscripcion(Guid eventoId)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();
                try
                {
                    var response = await connection.QueryAsync<FichaInscripcionResponse>(
                        "Sistema.sp_FichaInscripcionEventos",
                        new { EventoId = eventoId },
                        commandType: CommandType.StoredProcedure);

                    return response?.ToList() ?? new List<FichaInscripcionResponse>(); ;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sp_FichaInscripcionTelefonosPorEstadoAtencion", ex);
                }
            }
        }

        public async Task<int> CantidadPorEvento(Guid EventoId)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();
                try
                {
                    var response = await connection.QuerySingleOrDefaultAsync<int>(
                        "Sistema.sp_CantidadFichaInscripcionPorEvento",
                        new { EventoId = EventoId},
                        commandType: CommandType.StoredProcedure);

                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sp_CantidadFichaInscripcionPorEvento", ex);
                }
            }
        }

    }
}
