using CCL.CRMEnvioSMS.Core.Interface;
using CCL.CRMEnvioSMS.Core.Service;
using CCL.CRMEnvioSMS.Data.Interface;
using CCL.CRMEnvioSMS.Data.Repository;
using CCL.CRMEnvioSMS.Utility;
using CCL.CRMEnvioSMS.Utility.Model;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  
                   .AllowAnyMethod() 
                   .AllowAnyHeader();
        });
});*/
builder.Services.AddHttpClient();
builder.Services.AddScoped<Settings>();

builder.Services.AddScoped<IFichaInscripcionService, FichaInscripcionService>();
builder.Services.AddScoped<ISolicitudSMSMasivoService, SolicitudSMSMasivoService>();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddScoped<ICampaignSolicitudSMSService, CampaignSolicitudSMSService>();


builder.Services.AddScoped<IFichaInscripcionRepository, FichaInscripcionRepository>();
builder.Services.AddScoped<ISolicitudSMSMasivoRepository, SolicitudSMSMasivoRepository>();
builder.Services.AddScoped<ISolicitudSMSMasivoNumeroRepository, SolicitudSMSMasivoNumeroRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<ICampaingSMSNumerosRepository, CampaingSMSNumerosRepository>();
builder.Services.AddScoped<ICampaignSolicitudSMSRepository, CampaingSolicitudSMSRepository>();
builder.Services.Configure<EnviaMasSettings>(builder.Configuration.GetSection("EnviaMas"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*app.UseCors("AllowAll");*/

app.UseAuthorization();

app.MapControllers();

app.Run();
