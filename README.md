# Starter do Modulo: MCP para Desenvolvedores .NET 10

Estrutura de partida para os laboratorios do modulo de 32h. **Compilar e correr
localmente e parte do Lab 1/3**: este starter foi gerado contra a documentacao
oficial do SDK C# v2 (linha 2.x), mas **nao foi compilado neste ambiente** --
o primeiro passo e validar `dotnet build` e ajustar a versao dos pacotes se o
NuGet devolver versoes mais recentes.

## Pre-requisitos

- .NET 10 SDK
- VS Code com C# Dev Kit (ou Visual Studio 2022 17.12+)
- Node.js (para o MCP Inspector: `npx @modelcontextprotocol/inspector`)

## Estrutura

| Projeto | Papel | Lab |
|---|---|---|
| `src/TreinamentoMcp.Dominio` | Caso de uso puro: `Tarefa`, `IRepositorioTarefas`, regras de negocio. A fachada MCP deve ficar fina sobre isto. | Lab 4A |
| `src/TreinamentoMcp.ServidorStdio` | Servidor MCP por stdio (`AddMcpServer().WithStdioServerTransport().WithToolsFromAssembly()`), com 3 tools + 1 resource template. Log apenas para stderr. | Lab 3A |
| `src/TreinamentoMcp.ServidorHttp` | O mesmo dominio exposto por Streamable HTTP (`AddMcpServer().WithHttpTransport(o => o.Stateless = true)` + `MapMcp()`), `AllowedHosts` restringido. | Lab 3B |
| `src/TreinamentoMcp.Cliente` | Cliente MCP baseado no exemplo oficial (`McpClient.CreateAsync` + `StdioClientTransport`): lista tools e chama uma. | Lab 4B |
| `tests/TreinamentoMcp.TestesSmoke` | xUnit que se liga ao servidor stdio como um host real: descoberta de tools, ciclo criar/listar/concluir e mensagem de erro de negocio via `McpException`. | Lab 3/4 |
| `.vscode/mcp.json` | Configuracao dos dois servidores no VS Code (stdio + http). | Lab 3B |

**Nota pedagogica:** as classes de tools estao duplicadas em cada servidor de
proposito (`WithToolsFromAssembly()` varre o proprio assembly). Nos labs 4-5 o
exercicio e extrai-las para uma fachada comum.

## Como compilar e correr

```bash
dotnet build
dotnet run --project src/TreinamentoMcp.ServidorStdio        # stdio (usar via Inspector/mcp.json)
dotnet run --project src/TreinamentoMcp.ServidorHttp         # HTTP em http://localhost:3001
dotnet test                                                  # smoke tests (ligam ao servidor stdio)
dotnet run --project src/TreinamentoMcp.Cliente              # cliente base + listagem de tools
```

## Testar no MCP Inspector

```bash
npx @modelcontextprotocol/inspector dotnet run --project src/TreinamentoMcp.ServidorStdio
```

Para o HTTP: abrir o Inspector no modo HTTP e apontar para `http://localhost:3001`.

## Testar no VS Code (Copilot)

`.vscode/mcp.json` ja pre-configura os dois servidores. Para o stdio, compilar
primeiro (`dotnet build`) e depois arrancar o servidor de MCP no painel do VS Code.

## Pontos "a verificar" se a versao do SDK mudar

- Versoes dos pacotes em `Directory.Packages.props` (linha 2.x; ajustar a patch mais recente).
- Se `net10.0` nao existir no vosso SDK, ainda sem .NET 10: trocar para `net9.0`
  (o SDK 2.x tambem o suporta) -- mas o modulo assume .NET 10.
- Nomes de ferramentas: o starter nao fixa nomes explicitos; os testes procuram
  por conteudo do nome ("criar"/"listar"/"concluir") de proposito, tolerante ao
  nominal default do SDK. Se fixarem `ToolName` nos atributos, atualizar o cliente.
- O repositorio em memoria recomeca vazio a cada reinicio do servidor (esperado).

## TODO dos labs (marcados no codigo)

1. Lab 3A: `PromptsTarefas.cs` - implementar o prompt "resumo-do-dia".
2. Lab 3B: migrar/auto-associar autenticacao JWT no servidor HTTP (Lab 5).
3. Lab 4B: `src/TreinamentoMcp.Cliente` - adicionar integracao com `IChatClient`
   (Microsoft.Extensions.AI), passando `McpClientTool` como `Tools`.
4. Lab 5: rate limiting (`AddRateLimiter`), autorizacao por tool, auditoria.
