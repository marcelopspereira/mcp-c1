using ModelContextProtocol.Server;
using TreinamentoMcp.Dominio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepositorioTarefas, RepositorioTarefasEmMemoria>();

builder.Services
    .AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.Stateless = true;
    })
    .WithToolsFromAssembly();

var app = builder.Build();

app.MapMcp();

app.Run("http://localhost:3001");
