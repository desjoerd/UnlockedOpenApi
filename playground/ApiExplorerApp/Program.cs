using ApiExplorerApp.Explorer;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ExploredAppProvider>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapExplorer();

app.Run();

internal partial class Program
{
}