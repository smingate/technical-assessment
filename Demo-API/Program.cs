using Demo_API.Iservice;
using Demo_API.Service;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITargetAssetService, TargetAssetService>();
builder.Services.AddHttpClient<ITargetAssetService, TargetAssetService>(client =>
{
    client.BaseAddress = new Uri("https://06ba2c18-ac5b-4e14-988c-94f400643ebf.mock.pstmn.io/targetAsset");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();