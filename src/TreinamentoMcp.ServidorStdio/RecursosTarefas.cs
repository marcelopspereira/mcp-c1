using System.ComponentModel;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using TreinamentoMcp.Dominio;

namespace TreinamentoMcp.ServidorStdio;

[McpServerResourceType]
public static class RecursosTarefas
{
    [McpServerResource(UriTemplate = "tarefas://{id}", Name = "Tarefa", MimeType = "application/json")]
    [Description("Devolve uma tarefa pelo seu id em formato compacto.")]
    public static async Task<TextResourceContents> ObterTarefa(
        IRepositorioTarefas repositorio,
        Guid id)
    {
        var tarefa = await repositorio.ObterAsync(id)
            ?? throw new McpException($"Nao existe tarefa com o id {id}.");

        return new TextResourceContents
        {
            Uri = $"tarefas://{id}",
            MimeType = "application/json",
            Text = $"{tarefa.Titulo} | {tarefa.Estado} | {tarefa.Proprietario}"
        };
    }
}
