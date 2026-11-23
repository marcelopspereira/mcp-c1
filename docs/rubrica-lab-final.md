# Rubrica de avaliação — Laboratório Final Integrador ("Assistente de Operações")

Pontuação total: 100 pontos (nota final = pontos). Limiar de aprovação: 70.
Métodos de verificação: `auto` (participante demonstra com captura/log),
`pares` (outro grupo audita a vossa demo), `instrutor` (avaliação direta).

## Bloco A — Arquitetura e contratos MCP (25 pts)

| # | Critério verificável | Verificação | Pts |
|---|---|---|---|
| A1 | Cada servidor tem ≤ 6 tools, cada uma mapeia um caso de uso (não um CRUD); a grelha de decisão tool/resource/prompt existe, com justificação por item. | instrutor (doc de desenho + código) | 8 |
| A2 | Zero lógica de negócio no MCP server: chamadas inválidas (tarefa inexistente, tarefa de outro utilizador, tarefa já concluída) devolvem `McpException` cuja mensagem vem do domínio; não há validações duplicadas na fachada. | instrutor (revisão de código + teste) | 8 |
| A3 | Todas as tools têm `[Description]` em todos os parâmetros; saídas compactas (formato curto, sem DTOs de domínio completos). | pares (inspecionam `tools/list`) | 6 |
| A4 | Erros de negócio chegam ao modelo como resultado com `IsError = true` e mensagem legível, nunca stack trace. | auto (captura no Inspector) | 3 |

## Bloco B — Funcionalidade ponta a ponta (25 pts)

| # | Critério verificável | Verificação | Pts |
|---|---|---|---|
| B1 | Os 3 cenários de composição (2 servidores MCP distintos, ≥ 2 tools envolvidas por cenário) executam com resultado correto no VS Code/Copilot. | instrutor (demo) | 8 |
| B2 | Os mesmos 3 cenários executam no back-end .NET próprio (host via SDK) com front-end web a mostrar a conversa e a tool escolhida. | instrutor (demo) | 8 |
| B3 | O servidor do grupo-parceiro foi consumido sem qualquer alteração nele (diff do repo do parceiro limpo na fase de integração). | pares (revisor confirma) | 5 |
| B4 | Falha do servidor-parceiro (parado propositadamente pelo instrutor) produz degradação com mensagem clara ao modelo, não uma exceção crua nem um bloqueio. | instrutor | 4 |

## Bloco C — Segurança (25 pts)

| # | Critério verificável | Verificação | Pts |
|---|---|---|---|
| C1 | Sem token → 401; token sem scope → 403; validação de `aud` ativa no endpoint MCP. | auto (captura curl/Inspector) | 6 |
| C2 | Escrita exige permissão distinta; utilizador A nunca vê dados de B, mesmo com prompts engenhosos (payloads forjados fornecidos pelo instrutor). | instrutor | 5 |
| C3 | Rate limit por utilizador demonstrado: 429 reproduzível, recupera após janela; política separada para tools caras. | auto (script) | 5 |
| C4 | Audit trail por chamada: utilizador, app-id (host), servidor, tool e argumentos — sem tokens nem credenciais nos logs. | instrutor (inspeção de logs) | 5 |
| C5 | Vetor de prompt injection pré-definido (payload fornecido) não provoca efeito de escrita; mitigação identificada e explicada. | instrutor (exercício de ataque) | 4 |

## Bloco D — Qualidade, testes e entrega (25 pts)

| # | Critério verificável | Verificação | Pts |
|---|---|---|---|
| D1 | `dotnet test` verde ligado a um host real (cliente do SDK): ≥ 2 tools validadas end-to-end + 1 caso de erro. | instrutor (executa o comando) | 8 |
| D2 | ADR de meia página: escolha tool nativa/MCP/OpenAPI + decisão OBO vs. service token, com os critérios das Aulas 3 e 10. | instrutor | 5 |
| D3 | Documento "vetor → controle → camada" com ≥ 6 entradas, mapeadas à implementação real do grupo. | instrutor | 4 |
| D4 | Demo em ≤ 15 min, no VS Code e no front-end, com rol do que compõe a solução (servidores, hosts, gateway). | instrutor | 5 |
| D5 | Nenhum segredo em código/config (user-secrets/variáveis de ambiente); `.gitignore` correto. | instrutor (scan) | 3 |

## Bónus (máx. +10; não repara reprovação)

| # | Critério | Pts |
|---|---|---|
| X1 | Resource com subscrição/notificação de mudança a funcionar (modo de sessões adequado). | +3 |
| X2 | Prompt MCP "resumo-operacional" invocável pelo nome nos dois hosts. | +2 |
| X3 | Relatório de auditoria consultável (query) que responde a "quem executou que tool, quando". | +3 |
| X4 | Contentorização do servidor HTTP + permitir correr atrás de um gateway local (YARP ou nginx) com política demonstrada. | +2 |

## Penalidades / reprovação automática

- Segredo em código ou commit: −15 pontos.
- Regras de negócio validadas apenas na fachada MCP (domínio contornado): A2 zerada e B1 limitado a 4 pontos.
- Consumo do parceiro com alterações ao servidor dele: B3 zerada.
- Demo com ferramentas de escrita sem autorização: bloco C inteiro a zero.

## Procedimento de avaliação

1. Entrega até 30 min antes da Sessão 8 (tag no repositório + relatórios em `docs/`).
2. Ordem de avaliação: D1 → A → B → C → demo final.
3. Os payloads de ataque do instrutor só são revelados no momento da avaliação do bloco C.
