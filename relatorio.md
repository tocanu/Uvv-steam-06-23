# Relatório – Atividade UML-GUI-Jogos (UVV Steam)

## Jogos escolhidos
- Jogo 06: Punch-Out (cópia de referência)
- Jogo 23: Snake clássico

## Requisitos obrigatórios (Atividade 01) e evidências
- Dois jogos escolhidos e implementados em C# Windows Forms.
- Novas funcionalidades adicionadas em ambos os jogos.
- Contribuições documentadas com “antes/depois”.
- Diagramas UML (casos de uso e classes) para cada jogo.
- Planejamento registrado com histórico das atividades.

## Contribuições e melhorias (antes/depois)
- Observações “antes”: `observacoes_jogos.md`
- Melhorias propostas e implementadas: `melhorias_jogos.md`

## Melhorias implementadas (resumo objetivo)

Jogo 06 (Punch-Out)
- Menu principal com opções de jogar, instruções e sair.
- Tela de instruções com controles.
- Tela de fim com botões (Reiniciar/Menu) e contagem de vitórias/derrotas.
- Pausa (P) e reinício rápido (R).
- Variação simples no movimento do inimigo.
- Ajuste de layout para funcionar em fullscreen.

Jogo 23 (Snake)
- Seletor de velocidade (Lento/Normal/Rápido).
- Velocidade progressiva opcional.
- Modo parede opcional.
- Pausa (P) e reinício rápido (R).
- Tempo de partida e status na tela.
- Ajuste de layout para funcionar em fullscreen.

## UML (diagramas)

Casos de uso
- `uml_casos_uso_jogo06.puml`
- `uml_casos_uso_jogo06.png`
- `uml_casos_uso_jogo23.puml`
- `uml_casos_uso_jogo23.png`

Diagrama de classes
- `uml_classes_jogo06.puml`
- `uml_classes_jogo06.png`
- `uml_classes_jogo23.puml`
- `uml_classes_jogo23.png`

## Histórico do planejamento
- Data: 11/03/2026 | Rodar jogos originais e observar funcionamento.
- Data: 17/03/2026 | Registrar problemas e limitações percebidas.
- Data: 17/03/2026 | Listar melhorias e novas funções.
- Data: 17/03/2026 | Implementar melhorias do Jogo 06.
- Data: 17/03/2026 | Implementar melhorias do Jogo 23.
- Data: 17/03/2026 | Produzir diagramas UML.
- Data: 17/03/2026 | Revisar documentação final.
Evidência do planejamento (Jira): `https://arthurfranca.atlassian.net/jira/software/projects/KAN/boards/1`

## Como executar

Jogo 06 (Punch-Out)
```powershell
dotnet run --project ".\Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\Simple Punch Out Game MOO ICT.csproj"
```

Jogo 23 (Snake)
```powershell
dotnet run --project ".\Windows-Form-Snakes-Game-Tutorial-with-c-sharp-main\Classic Snakes Game Tutorial - MOO ICT\Classic Snakes Game Tutorial - MOO ICT.csproj"
```
