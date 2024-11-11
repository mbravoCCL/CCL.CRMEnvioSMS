using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Entity.Models.Response;
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
    public class EventoRepository : IEventoRepository
    {
        private readonly string conexionSQL;

        public EventoRepository(Settings connection)
        {
            conexionSQL = connection.getConnectionCRMProd();
        }

        public async Task<EventoResponse> Obtener(Guid EventoId)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();
                try
                {
                    var response = await connection.QuerySingleAsync<EventoResponse>(
                                     "Sistema.sp_ObtenerEventoPorId",
                                     new { EventoId = EventoId },
                                     commandType: CommandType.StoredProcedure);

                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sp_ObtenerEventoPorId", ex);
                }
            }
        }
    }
}
