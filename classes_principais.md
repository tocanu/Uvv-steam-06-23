# Classes principais dos jogos — explicação detalhada

Este arquivo abre cada classe e explica, em linguagem simples, o que cada parte faz.

## Jogo 06 (Punch-Out)

### Classes já existentes (originais)

**Program**
Onde: `Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\Program.cs`
O que é: é o ponto de partida do jogo. É o arquivo que “liga” a aplicação.
Detalhes:
- `[STAThread]`: indica que a janela usa o modelo de thread exigido pelo Windows Forms.
- `Main()`: é o primeiro método que roda quando o jogo inicia.
- `ApplicationConfiguration.Initialize()`: aplica configurações padrão do Windows Forms (ex.: fontes e DPI).
- `Application.Run(new MainMenuForm())`: abre o menu principal e mantém o programa rodando até o usuário fechar.

**Form1**
Onde: `Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\Form1.cs`
O que é: é a classe onde o combate acontece. Ela controla ataques, vida, pausa, fim de jogo e layout na tela.
Detalhes dos campos:
- `random`: gerador aleatório usado para escolher o ataque do inimigo.
- `enemyAttack`: lista das ações possíveis do inimigo (`left`, `right`, `block`).
- `playerBlock`: indica se o jogador está bloqueando agora.
- `enemyBlock`: indica se o inimigo está bloqueando agora.
- `enemySpeed`: velocidade horizontal atual do inimigo.
- `playerHealth`: vida do jogador.
- `enemyHealth`: vida do inimigo.
- `wins`: contador de vitórias.
- `losses`: contador de derrotas.
- `isPaused`: indica se o jogo está pausado.
- `isGameOver`: indica se a partida terminou.
- `moveTickCounter`: contador de atualizações para variar a movimentação do inimigo.
- `currentScale`: escala usada para ajustar o jogo quando a janela muda de tamanho.
- `enemyMinX` e `enemyMaxX`: limites esquerdo/direito do inimigo na tela.
- `BaseEnemySpeed`: velocidade base do inimigo (usada para calcular a escala).
- `BaseClientSize`: tamanho original da janela, usado como referência para o layout.
- `BasePlayerPos`: posição original do jogador.
- `BaseBoxerPos`: posição original do inimigo.
- `BaseBoxerHealthPos`: posição original da barra de vida do inimigo.
- `BasePlayerHealthPos`: posição original da barra de vida do jogador.
- `BaseHealthSize`: tamanho original das barras de vida.
- `statusLabel`: texto na tela que mostra vitórias e derrotas.
- `overlayLabel`: texto grande no meio da tela (ex.: “PAUSADO”, “VITÓRIA”, “DERROTA”).
- `gameOverDelayTimer`: timer usado para dar um pequeno intervalo antes de abrir a tela de fim.
- `pendingResult`: guarda se o resultado foi vitória ou derrota, antes de abrir a tela final.
- `GameOverResult`: tipo interno que indica “Win” ou “Lose”.
Detalhes dos métodos:
- `Form1()`: constrói a tela do jogo, liga o teclado, cria textos extras, configura o timer de fim e inicia o jogo.
- `InitializeExtraUi()`: cria o placar e a mensagem grande que aparece no meio da tela.
- `InitializeDelayTimer()`: prepara o timer que dá a pausa curta antes da tela de fim.
- `ApplyLayout()`: recalcula posições e tamanhos quando a janela muda, para evitar fullscreen quebrado.
- `UpdateScoreLabel()`: atualiza o texto de vitórias/derrotas.
- `UpdateScoreLabelPosition(...)`: posiciona o placar no topo da tela.
- `CenterOverlay()`: centraliza o texto grande (pausa/vitória/derrota).
- `ShowOverlay(...)`: mostra o texto grande.
- `HideOverlay()`: esconde o texto grande.
- `BoxerAttackTImerEvent(...)`: evento do timer de ataque do inimigo; sorteia a ação e aplica dano se houver contato.
- `BoxerMoveTimerEvent(...)`: evento do timer de movimento; move o inimigo, varia a velocidade e verifica fim de jogo.
- `KeyIsDown(...)`: lê o teclado; setas atacam/bloqueiam, `P` pausa e `R` reinicia.
- `KeyIsUp(...)`: soltar a tecla volta o jogador para a pose normal.
- `TogglePause()`: pausa ou despausa o jogo.
- `TriggerGameOver(...)`: prepara o fim de partida e inicia o timer de espera.
- `GameOverDelayTimer_Tick(...)`: abre a tela de fim e decide se reinicia ou volta ao menu.
- `ResetGame()`: reinicia tudo (vidas, posições, imagens, timers).
- `UpdateHealthBars()`: atualiza as barras de vida sem sair do limite 0–100.
- `DamagePlayer(...)`: reduz a vida do jogador.
- `DamageEnemy(...)`: reduz a vida do inimigo.
- `OnFormClosing(...)`: garante que timers parem ao fechar a janela.

**Form1 (parte de UI)**
Onde: `Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\Form1.Designer.cs`
O que é: a parte visual fixa do jogo (controles e timers).
Detalhes dos controles:
- `boxerHealthBar`: barra de vida do inimigo.
- `playerHealthBar`: barra de vida do jogador.
- `player`: imagem do jogador na tela.
- `boxer`: imagem do inimigo na tela.
- `BoxerAttackTimer`: timer que dispara os ataques do inimigo a cada 500 ms.
- `BoxerMoveTimer`: timer que move o inimigo a cada 20 ms.
Detalhes da janela:
- `BackgroundImage`: imagem de fundo do ringue.
- `BackgroundImageLayout = Stretch`: estica o fundo para caber na janela.
- `ClientSize = 734 x 561`: tamanho interno usado como referência.
- `DoubleBuffered = true`: reduz flicker (tremulação) na animação.
- `KeyDown` e `KeyUp`: conectam o teclado aos métodos do jogo.

### Novas classes adicionadas (melhorias)

**MainMenuForm**
Onde: `Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\MainMenuForm.cs`
O que é: a tela de menu principal. Mostra botões para jogar, ver instruções e sair.
Detalhes:
- Cria botões simples e centraliza tudo na tela.
- Quando o usuário clica em “Jogar”, abre o `Form1`.
- Quando clica em “Instruções”, abre a tela de instruções.

**InstructionsForm**
Onde: `Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\InstructionsForm.cs`
O que é: tela simples com os controles e o objetivo.
Detalhes:
- Exibe texto com comandos (setas, `P`, `R`).
- Tem botão de fechar para voltar ao menu.

**GameOverForm**
Onde: `Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\GameOverForm.cs`
O que é: tela de fim de jogo com o resultado.
Detalhes:
- Mostra “Você venceu!” ou “Você perdeu!”.
- Mostra vitórias e derrotas acumuladas.
- Possui botões para reiniciar ou voltar ao menu.

**GameOverAction**
Onde: `Simple-Punch-Out-Game-MOO-ICT-master\Simple Punch Out Game MOO ICT\GameOverForm.cs`
O que é: um tipo auxiliar que indica a escolha do usuário (`Restart` ou `Menu`).

## Jogo 23 (Snake)

### Classes já existentes (originais)

**Program**
Onde: `Windows-Form-Snakes-Game-Tutorial-with-c-sharp-main\Classic Snakes Game Tutorial - MOO ICT\Program.cs`
O que é: ponto de entrada do jogo.
Detalhes:
- `[STAThread]`: necessário para Windows Forms.
- `Main()`: inicia a aplicação.
- `ApplicationConfiguration.Initialize()`: configurações padrão do Windows Forms.
- `Application.Run(new Form1())`: cria a janela principal do jogo.

**Form1**
Onde: `Windows-Form-Snakes-Game-Tutorial-with-c-sharp-main\Classic Snakes Game Tutorial - MOO ICT\Form1.cs`
O que é: onde toda a lógica do Snake acontece, incluindo velocidade, pausa, tempo e modo parede.
Detalhes dos campos:
- `Snake`: lista de círculos que formam o corpo da cobra. O primeiro é a cabeça.
- `food`: um círculo que representa a comida.
- `maxWidth`, `maxHeight`: limites máximos do tabuleiro em células.
- `score`: pontuação atual.
- `highScore`: maior pontuação alcançada.
- `rand`: gerador de números aleatórios para posicionar comida.
- `goLeft`, `goRight`, `goDown`, `goUp`: flags que guardam a direção pressionada.
- `isPaused`: indica se o jogo está pausado.
- `wallMode`: define se bater na parede encerra o jogo.
- `progressiveSpeed`: define se a velocidade aumenta com a pontuação.
- `baseInterval`: intervalo base do timer (define a velocidade inicial).
- `minInterval`: limite mínimo de intervalo para não ficar rápido demais.
- `elapsedSeconds`: tempo total da partida em segundos.
- `clockTimer`: timer que conta o tempo da partida.
- `txtTime`: label que mostra o tempo na tela.
- `txtStatus`: label que mostra o status (jogando, pausado, game over).
- `speedLabel`: label do seletor de velocidade.
- `speedCombo`: combo para escolher a velocidade inicial.
- `wallModeCheck`: checkbox para ativar modo parede.
- `progressiveCheck`: checkbox para ativar velocidade progressiva.
Detalhes dos métodos:
- `Form1()`: construtor. Monta a interface, ativa teclado, cria os controles extras e ajusta o layout.
- `KeyIsDown(...)`: lê o teclado. `P` pausa, `R` reinicia e setas mudam a direção.
- `KeyIsUp(...)`: soltar a tecla desativa o movimento daquela direção.
- `StartGame(...)`: evento do botão Start. Chama `RestartGame()`.
- `TakeSnapShot(...)`: cria um texto com a pontuação, desenha na tela e salva uma imagem JPG do jogo.
- `GameTimerEvent(...)`: atualiza a direção, move a cobra e verifica colisões.
- `GameTimerEvent(...)` com `wallMode`: se bater na parede, chama `GameOver()`.
- `GameTimerEvent(...)` sem `wallMode`: a cobra atravessa a borda e reaparece do outro lado.
- `UpdatePictureBoxGraphics(...)`: desenha a cobra e a comida no `picCanvas`.
- `RestartGame()`: reseta a partida, zera tempo, reinicia direção e timers.
- `EatFood()`: aumenta pontuação, adiciona um pedaço na cobra e ajusta a velocidade.
- `GameOver()`: para o jogo, para o relógio e mostra status de fim.
- `InitializeExtras()`: cria os controles extras (velocidade, modo parede, tempo, status).
- `UpdateSpeedFromSelection()`: traduz a escolha do combo em intervalo base.
- `ApplySpeed()`: calcula a velocidade final do jogo considerando a pontuação.
- `ClockTimer_Tick(...)`: soma 1 segundo no tempo quando o jogo está rodando.
- `UpdateTimeLabel()`: atualiza o texto do tempo no formato mm:ss.
- `TogglePause()`: pausa ou continua o jogo.
- `ApplyLayout()`: redimensiona canvas e reposiciona controles (corrige fullscreen).
- `UpdateBoardSize()`: recalcula o tamanho do tabuleiro baseado no canvas.

**Form1 (parte de UI)**
Onde: `Windows-Form-Snakes-Game-Tutorial-with-c-sharp-main\Classic Snakes Game Tutorial - MOO ICT\Form1.Designer.cs`
O que é: define os controles base e o timer do jogo.
Detalhes dos controles:
- `startButton`: inicia o jogo.
- `snapButton`: salva uma imagem da tela.
- `picCanvas`: área onde a cobra e a comida são desenhadas.
- `txtScore`: mostra a pontuação atual.
- `txtHighScore`: mostra o recorde.
- `gameTimer`: timer que controla a velocidade base do jogo.
Detalhes da janela:
- `ClientSize = 748 x 725`: tamanho interno usado como referência.
- `KeyDown` e `KeyUp`: conectam o teclado ao jogo.

**Circle**
Onde: `Windows-Form-Snakes-Game-Tutorial-with-c-sharp-main\Classic Snakes Game Tutorial - MOO ICT\Circle.cs`
O que é: um “pedaço” da cobra (e também a comida).
Detalhes:
- `X` e `Y`: posição do círculo no tabuleiro.
- Construtor: começa com `X = 0` e `Y = 0`.

**Settings**
Onde: `Windows-Form-Snakes-Game-Tutorial-with-c-sharp-main\Classic Snakes Game Tutorial - MOO ICT\Settings.cs`
O que é: guarda configurações globais do jogo.
Detalhes:
- `Width` e `Height`: tamanho de cada célula do tabuleiro.
- `directions`: direção atual da cobra.
- Construtor: define `Width = 16`, `Height = 16` e direção inicial `left`.

### Novas classes adicionadas (melhorias)

- Nenhuma até o momento.
