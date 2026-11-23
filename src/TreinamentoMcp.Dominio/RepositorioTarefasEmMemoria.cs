using System.Collections.Concurrent;

namespace TreinamentoMcp.Dominio;

public class RepositorioTarefasEmMemoria : IRepositorioTarefas
{
    private readonly ConcurrentDictionary<Guid, Tarefa> _tarefas = new();

    public Task<Tarefa> CriarAsync(string titulo, string proprietario, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titulo);
        ArgumentException.ThrowIfNullOrWhiteSpace(proprietario);

        var tarefa = new Tarefa(Guid.NewGuid(), titulo.Trim(), proprietario, EstadoTarefa.Aberta, DateTimeOffset.UtcNow);
        _tarefas[tarefa.Id] = tarefa;
        return Task.FromResult(tarefa);
    }

    public Task<IReadOnlyList<Tarefa>> ListarAsync(string proprietario, EstadoTarefa? estado, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(proprietario);

        var resultado = _tarefas.Values
            .Where(t => t.Proprietario == proprietario)
            .Where(t => estado is null || t.Estado == estado)
            .OrderBy(t => t.CriadaEm)
            .ToList();
        return Task.FromResult<IReadOnlyList<Tarefa>>(resultado);
    }

    public Task<Tarefa?> ObterAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_tarefas.TryGetValue(id, out var tarefa) ? tarefa : null);

    public Task<Tarefa> ConcluirAsync(Guid id, string proprietario, CancellationToken cancellationToken = default)
    {
        var tarefa = _tarefas.TryGetValue(id, out var existente) ? existente : null;

        if (tarefa is null)
            throw new RegrasNegocioException($"Nao existe tarefa com o id {id}.");

        if (tarefa.Proprietario != proprietario)
            throw new RegrasNegocioException("A tarefa pertence a outro utilizador.");

        if (tarefa.Estado == EstadoTarefa.Concluida)
            throw new RegrasNegocioException($"A tarefa '{tarefa.Titulo}' ja esta concluida.");

        var concluida = tarefa with { Estado = EstadoTarefa.Concluida, ConcluidaEm = DateTimeOffset.UtcNow };
        _tarefas[id] = concluida;
        return Task.FromResult(concluida);
    }
}
