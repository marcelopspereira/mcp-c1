using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace TreinamentoMcp.TestesSmoke;

public class TestesSmokeServidor : IAsyncLifetime
{
    private ServidorTarefas _servidor = null!;

    public async Task InitializeAsync() => _servidor = await ServidorTarefas.CriarAsync();

    public async Task DisposeAsync()
    {
        if (_servidor is not null)
            await _servidor.DisposeAsync();
    }

    [Fact]
    public async Task ServidorExpoeFerramentasTarefas()
    {
        var tools = await _servidor.Cliente.ListToolsAsync();

        Assert.Contains(tools, t => t.Name.Contains("criar", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(tools, t => t.Name.Contains("listar", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(tools, t => t.Name.Contains("concluir", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CicloCriarListarConcluirFunciona()
    {
        var cliente = _servidor.Cliente;
        var tools = await cliente.ListToolsAsync();

        var criar = tools.First(t => t.Name.Contains("criar", StringComparison.OrdinalIgnoreCase));
        var listar = tools.First(t => t.Name.Contains("listar", StringComparison.OrdinalIgnoreCase));
        var concluir = tools.First(t => t.Name.Contains("concluir", StringComparison.OrdinalIgnoreCase));

        var criada = await cliente.CallToolAsync(criar.Name,
            new Dictionary<string, object?> { ["titulo"] = "Preparar Lab 3" },
            cancellationToken: CancellationToken.None);
        var textoCriada = criada.Content.OfType<TextContentBlock>().First().Text;
        var idInicio = textoCriada.IndexOf(" | ");
        Assert.True(idInicio > 0);
        var id = textoCriada[..idInicio];

        var listagem = await cliente.CallToolAsync(listar.Name,
            new Dictionary<string, object?> { ["proprietario"] = "aluno-demo" },
            cancellationToken: CancellationToken.None);
        Assert.Contains("Preparar Lab 3", listagem.Content.OfType<TextContentBlock>().First().Text);

        var concluida = await cliente.CallToolAsync(concluir.Name,
            new Dictionary<string, object?> { ["id"] = id, ["proprietario"] = "aluno-demo" },
            cancellationToken: CancellationToken.None);
        Assert.Contains("Concluida", concluida.Content.OfType<TextContentBlock>().First().Text);
    }

    [Fact]
    public async Task ConcluirTarefaDuplicadaDevolveErroComMensagemDeNegocio()
    {
        var cliente = _servidor.Cliente;
        var tools = await cliente.ListToolsAsync();

        var criar = tools.First(t => t.Name.Contains("criar", StringComparison.OrdinalIgnoreCase));
        var concluir = tools.First(t => t.Name.Contains("concluir", StringComparison.OrdinalIgnoreCase));

        var criada = await cliente.CallToolAsync(criar.Name,
            new Dictionary<string, object?> { ["titulo"] = "Unica e nao duplicavel" },
            cancellationToken: CancellationToken.None);
        var textoCriada = criada.Content.OfType<TextContentBlock>().First().Text;
        var id = textoCriada[..textoCriada.IndexOf(" | ", StringComparison.Ordinal)];

        var primeira = await cliente.CallToolAsync(concluir.Name,
            new Dictionary<string, object?> { ["id"] = id, ["proprietario"] = "aluno-demo" },
            cancellationToken: CancellationToken.None);
        Assert.False(primeira.IsError);

        var segunda = await cliente.CallToolAsync(concluir.Name,
            new Dictionary<string, object?> { ["id"] = id, ["proprietario"] = "aluno-demo" },
            cancellationToken: CancellationToken.None);
        Assert.True(segunda.IsError);
        var textoErro = segunda.Content.OfType<TextContentBlock>().First().Text;
        Assert.Contains("ja esta concluida", textoErro);
    }}
EOF
ls