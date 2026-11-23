using ModelContextProtocol.Client;

namespace TreinamentoMcp.TestesSmoke;

public sealed class ServidorTarefas : IAsyncDisposable
{
    public McpClient Cliente { get; }

    private ServidorTarefas(McpClient cliente) => Cliente = cliente;

    public static async Task<ServidorTarefas> CriarAsync()
    {
        var raiz = EncontrarRaizSolucacao();
        var caminhoProjeto = Path.Combine(raiz, "src", "TreinamentoMcp.ServidorStdio", "TreinamentoMcp.ServidorStdio.csproj");

        var transporte = new StdioClientTransport(new StdioClientTransportOptions
        {
            Name = "Tarefas",
            Command = "dotnet",
            Arguments = ["run", "--project", caminhoProjeto, "--no-build"],
        });

        var cliente = await McpClient.CreateAsync(transporte);
        return new ServidorTarefas(cliente);
    }

    public async ValueTask DisposeAsync() => await Cliente.DisposeAsync();

    private static string EncontrarRaizSolucacao()
    {
        for (var atual = new DirectoryInfo(AppContext.BaseDirectory); atual is not null; atual = atual.Parent)
        {
            if (File.Exists(Path.Combine(atual.FullName, "McpModuloStarter.slnx")))
                return atual.FullName;
        }
        throw new InvalidOperationException("Raiz da solucao nao encontrada a partir dos testes.");
    }
}
