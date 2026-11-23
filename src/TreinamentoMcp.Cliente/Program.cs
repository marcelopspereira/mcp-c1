using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

var transporte = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "Tarefas",
    Command = "dotnet",
    Arguments = ["run", "--project", EncontrarProjetoServidor(), "--no-build"],
});

await using var cliente = await McpClient.CreateAsync(transporte);

var tools = await cliente.ListToolsAsync();
Console.WriteLine("Tools disponiveis:");
foreach (var tool in tools)
    Console.WriteLine($"  {tool.Name}: {tool.Description}");

var criar = ToolDe(tools, "criar");
var listar = ToolDe(tools, "listar");

var criada = await cliente.CallToolAsync(
    criar.Name,
    new Dictionary<string, object?> { ["titulo"] = "Instalar o MCP Inspector" },
    cancellationToken: CancellationToken.None);

Console.WriteLine();
Console.WriteLine("Resultado de CriarTarefa:");
Console.WriteLine(criada.Content.OfType<TextContentBlock>().First().Text);

var listagem = await cliente.CallToolAsync(
    listar.Name,
    new Dictionary<string, object?> { ["proprietario"] = "aluno-demo" },
    cancellationToken: CancellationToken.None);

Console.WriteLine();
Console.WriteLine("Resultado de ListarTarefas:");
Console.WriteLine(listagem.Content.OfType<TextContentBlock>().First().Text);

static McpClientTool ToolDe(IList<McpClientTool> tools, string fragmento)
    => tools.FirstOrDefault(t => t.Name.Contains(fragmento, StringComparison.OrdinalIgnoreCase))
       ?? throw new InvalidOperationException($"Tool '{fragmento}' nao encontrada no servidor.");

static string EncontrarProjetoServidor()
{
    for (var atual = new DirectoryInfo(AppContext.BaseDirectory); atual is not null; atual = atual.Parent)
    {
        if (File.Exists(Path.Combine(atual.FullName, "McpModuloStarter.slnx")))
            return Path.Combine(atual.FullName, "src", "TreinamentoMcp.ServidorStdio", "TreinamentoMcp.ServidorStdio.csproj");
    }

    throw new InvalidOperationException("Raiz da solucao nao encontrada.");
}
