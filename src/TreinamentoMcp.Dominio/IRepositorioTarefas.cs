namespace TreinamentoMcp.Dominio;

public interface IRepositorioTarefas
{
    Task<Tarefa> CriarAsync(string titulo, string proprietario, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Tarefa>> ListarAsync(string proprietario, EstadoTarefa? estado, CancellationToken cancellationToken = default);
    Task<Tarefa?> ObterAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tarefa> ConcluirAsync(Guid id, string proprietario, CancellationToken cancellationToken = default);
}
