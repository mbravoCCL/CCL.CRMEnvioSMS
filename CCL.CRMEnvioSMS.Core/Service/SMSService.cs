using CCL.CRMEnvioSMS.Core.Interface;
using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Entity.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CCL.CRMEnvioSMS.Entity.Models.Request;
using Microsoft.Extensions.Options;
using System.Net.Http;
using CCL.CRMEnvioSMS.Utility.Model;


namespace CCL.CRMEnvioSMS.Core.Service
{
    public class SMSService : ISMSService
    {

        private readonly HttpClient _httpClient;
        private readonly EnviaMasSettings _enviaMasSetting;
        public SMSService(IHttpClientFactory clientFactory,
            IOptions<EnviaMasSettings> enviaMasSetting)
        {
            _httpClient = clientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(enviaMasSetting.Value.Host);
            _enviaMasSetting = enviaMasSetting.Value;
        }

        public async Task<CampaignResponse> CreateCampaign(CampaignRequest request)
        {
            try
            {
                var endPoint = "api/sms/create_campaign";

                var byteArray = Encoding.ASCII.GetBytes($"{_enviaMasSetting.Email}:{_enviaMasSetting.Password}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(byteArray));

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endPoint, content);

                var resultResponse = await response.Content.ReadAsAsync<CampaignResponse>();
                return resultResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReportCampaingResponse> ReportCampaign(String campaingId)
        {
            try
            {
                var endPoint = $"api/sms/report_campaign/{campaingId}";

                var byteArray = Encoding.ASCII.GetBytes($"{_enviaMasSetting.Email}:{_enviaMasSetting.Password}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(byteArray));

                var response = await _httpClient.GetAsync(endPoint);

                var resultResponse = await response.Content.ReadAsStringAsync();
                var reportCampaingResponse = JsonSerializer.Deserialize<ReportCampaingResponse>(resultResponse);

                return reportCampaingResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
