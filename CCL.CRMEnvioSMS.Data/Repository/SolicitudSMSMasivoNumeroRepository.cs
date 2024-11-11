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
using CCL.CRMEnvioSMS.Entity.Models.Response;

namespace CCL.CRMEnvioSMS.Data.Repository
{
    public class SolicitudSMSMasivoNumeroRepository : ISolicitudSMSMasivoNumeroRepository
    {
        private readonly string conexionSQL;

        public SolicitudSMSMasivoNumeroRepository(Settings connection)
        {
            conexionSQL = connection.getConnectionEnviaMasSMS_CRM();
        }

        public async Task<bool> Registrar(Guid solicitudId, List<string> telefonos)
        {
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("new_solicituddesmsmasivoId", typeof(Guid));
            dataTable.Columns.Add("celular", typeof(string));


            foreach (var telefono in telefonos)
            {
                dataTable.Rows.Add(solicitudId, telefono);
            }

            using (var connection = new SqlConnection(conexionSQL))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Telefonos", dataTable.AsTableValuedParameter("dbo.TVP_SolicitudSMSMasivoNumeros"));

                var result = await connection.ExecuteAsync("dbo.sp_InsertarSolicitudSMSMasivoNumeros", parameters, commandType: CommandType.StoredProcedure);

                return result > 0;
            }
        }


        public async Task<List<string>> ListarTelefonosEnviados(Guid solicitudId)
        {
            using (var connection = new SqlConnection(conexionSQL))
            {
                await connection.OpenAsync();
                try
                {
                    var response = await connection.QueryAsync<string>(
                        "dbo.sp_ListarTelefonosPorSolicitud",
                        new { SolicitudId = solicitudId },
                        commandType: CommandType.StoredProcedure);

                    return response?.ToList() ?? new List<string>(); ;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sp_ListarTelefonosPorSolicitud", ex);
                }
            }
        }

    }
}
