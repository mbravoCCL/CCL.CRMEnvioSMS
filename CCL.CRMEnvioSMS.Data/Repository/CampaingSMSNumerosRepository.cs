using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.CRMEnvioSMS.Data.Repository
{
    public class CampaingSMSNumerosRepository : ICampaingSMSNumerosRepository
    {
        private readonly string conexionSQL;

        public CampaingSMSNumerosRepository(Settings connection)
        {
            conexionSQL = connection.getConnectionEnviaMasSMS_CRM();
        }

        public async Task<bool> Registrar(string campaign_id, List<string> telefonos)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("campaign_id", typeof(string));
            dataTable.Columns.Add("celular", typeof(string));


            foreach (var telefono in telefonos)
            {
                dataTable.Rows.Add(campaign_id, telefono);
            }

            using (var connection = new SqlConnection(conexionSQL))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@telefonos", dataTable.AsTableValuedParameter("dbo.TVP_CampaingSMSNumeros"));

                var result = await connection.ExecuteAsync("dbo.sp_InsertarCampaingSMSNumeros", parameters, commandType: CommandType.StoredProcedure);

                return result > 0;
            }
        }
    }
}
