namespace TreinamentoMcp.Dominio;

public enum EstadoTarefa
{
    Aberta,
    EmCurso,
    Concluida
}

public record Tarefa(
    Guid Id,
    string Titulo,
    string Proprietario,
    EstadoTarefa Estado,
    DateTimeOffset CriadaEm,
    DateTimeOffset? ConcluidaEm = null);
