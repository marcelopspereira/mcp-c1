using System.ComponentModel;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using TreinamentoMcp.Dominio;

namespace TreinamentoMcp.ServidorHttp;

[McpServerToolType]
public static class FerramentasTarefas
{
    [McpServerTool, Description("Cria uma nova tarefa para o utilizador indicado e devolve os seus dados.")]
    public static async Task<string> CriarTarefa(
        IRepositorioTarefas repositorio,
        [Description("Titulo curto e descritivo da tarefa")] string titulo,
        [Description("Identificador do proprietario da tarefa")] string proprietario = "aluno-demo")
    {
        var tarefa = await repositorio.CriarAsync(titulo, proprietario);
        return Format(tarefa);
    }

    [McpServerTool, Description("Lista tarefas do proprietario indicado, opcionalmente filtradas por estado (Aberta, EmCurso, Concluida).")]
    public static async Task<string> ListarTarefas(
        IRepositorioTarefas repositorio,
        [Description("Identificador do proprietario das tarefas")] string proprietario = "aluno-demo",
        [Description("Estado a filtrar; omissao devolve todas")] EstadoTarefa? estado = null)
    {
        var tarefas = await repositorio.ListarAsync(proprietario, estado);

        if (tarefas.Count == 0)
            return "Sem tarefas para o filtro indicado.";

        return string.Join(Environment.NewLine, tarefas.Select(Format));
    }

    [McpServerTool, Description("Marca uma tarefa Aberta ou EmCurso como concluida. Falha se a tarefa nao existir, pertencer a outro utilizador ou ja estiver concluida.")]
    public static async Task<string> ConcluirTarefa(
        IRepositorioTarefas repositorio,
        [Description("Id da tarefa a concluir")] Guid id,
        [Description("Identificador do proprietario da tarefa")] string proprietario = "aluno-demo")
    {
        var tarefa = await repositorio.ConcluirAsync(id, proprietario);
        return Format(tarefa);
    }

    private static string Format(Tarefa tarefa)
        => $"{tarefa.Id} | {tarefa.Titulo} | {tarefa.Estado} | {tarefa.Proprietario}";
}
