using CCL.CRMEnvioSMS.Core.Interface;
using CCL.CRMEnvioSMS.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCL.CRMEnvioSMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudSMSMasivoController : ControllerBase
    {
        private readonly ISolicitudSMSMasivoService _service;
        public SolicitudSMSMasivoController(ISolicitudSMSMasivoService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("envio/{idSolicitud}")]
        public async Task<IActionResult> smsMasivo(Guid idSolicitud)
        {
            try
            {
                
                var result = await _service.smsMasivo(idSolicitud);

                
                if (result.Campaign.Succcess)
                {
                    var htmlContent = $@"
            <html>
                <head>
                    <meta charset='UTF-8' />
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 50px auto;
                            padding: 20px;
                            background-color: #fff;
                            border-radius: 8px;
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                        }}
                        h1 {{
                            color: #4CAF50;
                            text-align: center;
                        }}
                        p {{
                            font-size: 16px;
                            color: #333;
                        }}
                        .success {{
                            color: #4CAF50;
                            font-weight: bold;
                        }}
                        .error {{
                            color: #D8000C;
                            font-weight: bold;
                        }}
                        .details {{
                            margin-top: 20px;
                            padding: 10px;
                            background-color: #f9f9f9;
                            border: 1px solid #ddd;
                            border-radius: 4px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Resultado del Envío</h1>
                        <p class='success'>SMS masivo ha sido procesado correctamente.</p>
                        <p><strong>Mensaje:</strong> {result.Campaign.Message}</p>
                        <div class='details'>
                            <p><strong>Campaign ID:</strong> {result.Campaign.Data.campaign_id}</p>
                            <p><strong>Nota importante:</strong> Si algún número no tiene el formato correcto, podría no haberle llegado el SMS.</p>
                            <p><strong>Números Registrados:</strong></p>
                            <ul>
                                {string.Join("", result.Telefonos.Select(t => $"<li>{t}</li>"))}
                            </ul>

                        </div>
                    </div>
                </body>
            </html>";

                    return Content(htmlContent, "text/html");
                }
                else
                {
                    var errorHtml = $@"
            <html>
                <head>
                    <meta charset='UTF-8' />
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 50px auto;
                            padding: 20px;
                            background-color: #fff;
                            border-radius: 8px;
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                        }}
                        h1 {{
                            color: #D8000C;
                            text-align: center;
                        }}
                        p {{
                            font-size: 16px;
                            color: #333;
                        }}
                        .error {{
                            color: #D8000C;
                            font-weight: bold;
                        }}
                        .details {{
                            margin-top: 20px;
                            padding: 10px;
                            background-color: #f9f9f9;
                            border: 1px solid #ddd;
                            border-radius: 4px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>¡Error al Enviar el SMS!</h1>
                        <p class='error'>Ocurrió un error al procesar el SMS masivo.</p>
                        <p><strong>Mensaje:</strong> {result.Campaign.Message}</p>
                    </div>
                </body>
            </html>";

                    return Content(errorHtml, "text/html");
                }
            }
            catch (Exception ex)
            {
                var errorHtml = $@"
        <html>
            <head>
                <meta charset='UTF-8' />
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 50px auto;
                        padding: 20px;
                        background-color: #fff;
                        border-radius: 8px;
                        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                    }}
                    h1 {{
                        color: #D8000C;
                        text-align: center;
                    }}
                    p {{
                        font-size: 16px;
                        color: #333;
                    }}
                    .error {{
                        color: #D8000C;
                        font-weight: bold;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>¡Error!</h1>
                    <p class='error'>Ocurrió un error al procesar la solicitud.</p>
                    <p><strong>Detalles del error:</strong> {ex.Message}</p>
                </div>
            </body>
        </html>";

 
                return Content(errorHtml, "text/html");
            }
        }


    }
}
