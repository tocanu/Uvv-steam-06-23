namespace Simple_Punch_Out_Game_MOO_ICT
{
    public partial class Form1 : Form
    {
        private readonly Random random = new Random();
        private readonly List<string> enemyAttack = new List<string> { "left", "right", "block" };

        private bool playerBlock;
        private bool enemyBlock;
        private int enemySpeed = 5;
        private int playerHealth = 100;
        private int enemyHealth = 100;

        private int wins;
        private int losses;

        private bool isPaused;
        private bool isGameOver;

        private int moveTickCounter;
        private float currentScale = 1f;
        private int enemyMinX;
        private int enemyMaxX;

        private const int BaseEnemySpeed = 5;
        private static readonly Size BaseClientSize = new Size(734, 561);
        private static readonly Point BasePlayerPos = new Point(348, 407);
        private static readonly Point BaseBoxerPos = new Point(404, 321);
        private static readonly Point BaseBoxerHealthPos = new Point(12, 43);
        private static readonly Point BasePlayerHealthPos = new Point(483, 43);
        private static readonly Size BaseHealthSize = new Size(239, 23);

        private Label statusLabel = null!;
        private Label overlayLabel = null!;
        private System.Windows.Forms.Timer gameOverDelayTimer = null!;
        private GameOverResult pendingResult;

        private enum GameOverResult
        {
            Win,
            Lose
        }

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;

            InitializeExtraUi();
            InitializeDelayTimer();

            Resize += (_, __) => ApplyLayout();

            ApplyLayout();
            ResetGame();
        }

        private void InitializeExtraUi()
        {
            statusLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Padding = new Padding(6, 3, 6, 3)
            };

            overlayLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Padding = new Padding(16, 8, 16, 8),
                Visible = false
            };

            Controls.Add(statusLabel);
            Controls.Add(overlayLabel);

            overlayLabel.BringToFront();
            statusLabel.BringToFront();

            UpdateScoreLabel();
        }

        private void InitializeDelayTimer()
        {
            gameOverDelayTimer = new System.Windows.Forms.Timer
            {
                Interval = 800
            };
            gameOverDelayTimer.Tick += GameOverDelayTimer_Tick;
        }

        private void ApplyLayout()
        {
            float scaleX = (float)ClientSize.Width / BaseClientSize.Width;
            float scaleY = (float)ClientSize.Height / BaseClientSize.Height;
            float scale = Math.Min(scaleX, scaleY);
            if (scale <= 0f)
            {
                scale = 1f;
            }

            currentScale = scale;

            int scaledWidth = (int)(BaseClientSize.Width * scale);
            int scaledHeight = (int)(BaseClientSize.Height * scale);
            int offsetX = (ClientSize.Width - scaledWidth) / 2;
            int offsetY = (ClientSize.Height - scaledHeight) / 2;

            player.Location = new Point(
                offsetX + (int)(BasePlayerPos.X * scale),
                offsetY + (int)(BasePlayerPos.Y * scale));

            boxer.Location = new Point(
                offsetX + (int)(BaseBoxerPos.X * scale),
                offsetY + (int)(BaseBoxerPos.Y * scale));

            boxerHealthBar.SetBounds(
                offsetX + (int)(BaseBoxerHealthPos.X * scale),
                offsetY + (int)(BaseBoxerHealthPos.Y * scale),
                Math.Max(100, (int)(BaseHealthSize.Width * scale)),
                Math.Max(12, (int)(BaseHealthSize.Height * scale)));

            playerHealthBar.SetBounds(
                offsetX + (int)(BasePlayerHealthPos.X * scale),
                offsetY + (int)(BasePlayerHealthPos.Y * scale),
                Math.Max(100, (int)(BaseHealthSize.Width * scale)),
                Math.Max(12, (int)(BaseHealthSize.Height * scale)));

            enemyMinX = offsetX + (int)(220 * scale);
            enemyMaxX = offsetX + (int)(430 * scale);

            int speedDirection = enemySpeed == 0 ? 1 : Math.Sign(enemySpeed);
            enemySpeed = speedDirection * Math.Max(1, (int)(BaseEnemySpeed * scale));

            UpdateScoreLabelPosition(offsetX, offsetY);
            CenterOverlay();
        }

        private void UpdateScoreLabel()
        {
            statusLabel.Text = $"Vitórias: {wins}  |  Derrotas: {losses}";
            UpdateScoreLabelPosition(0, 0);
        }

        private void UpdateScoreLabelPosition(int offsetX, int offsetY)
        {
            statusLabel.Left = (ClientSize.Width - statusLabel.Width) / 2;
            statusLabel.Top = Math.Max(5, offsetY + 5);
        }

        private void CenterOverlay()
        {
            overlayLabel.Left = (ClientSize.Width - overlayLabel.Width) / 2;
            overlayLabel.Top = (ClientSize.Height - overlayLabel.Height) / 2;
        }

        private void ShowOverlay(string text)
        {
            overlayLabel.Text = text;
            overlayLabel.Visible = true;
            CenterOverlay();
            overlayLabel.BringToFront();
        }

        private void HideOverlay()
        {
            overlayLabel.Visible = false;
        }

        private void BoxerAttackTImerEvent(object sender, EventArgs e)
        {
            if (isPaused || isGameOver)
            {
                return;
            }

            int index = random.Next(0, enemyAttack.Count);

            switch (enemyAttack[index])
            {
                case "left":
                    boxer.Image = Properties.Resources.enemy_punch1;
                    enemyBlock = false;

                    if (boxer.Bounds.IntersectsWith(player.Bounds) && playerBlock == false)
                    {
                        DamagePlayer(5);
                    }
                    break;

                case "right":
                    boxer.Image = Properties.Resources.enemy_punch2;
                    enemyBlock = false;

                    if (boxer.Bounds.IntersectsWith(player.Bounds) && playerBlock == false)
                    {
                        DamagePlayer(5);
                    }
                    break;

                case "block":
                    boxer.Image = Properties.Resources.enemy_block;
                    enemyBlock = true;
                    break;
            }
        }

        private void BoxerMoveTimerEvent(object sender, EventArgs e)
        {
            if (isPaused || isGameOver)
            {
                return;
            }

            UpdateHealthBars();

            moveTickCounter++;
            if (moveTickCounter % 50 == 0)
            {
                int magnitude = random.Next(3, 7);
                int scaled = Math.Max(1, (int)(magnitude * currentScale));
                int direction = enemySpeed == 0 ? 1 : Math.Sign(enemySpeed);
                enemySpeed = direction * scaled;
            }
            if (moveTickCounter % 120 == 0 && random.NextDouble() < 0.3)
            {
                enemySpeed = -enemySpeed;
            }

            boxer.Left += enemySpeed;

            if (boxer.Left > enemyMaxX)
            {
                boxer.Left = enemyMaxX;
                enemySpeed = -Math.Abs(enemySpeed);
            }
            if (boxer.Left < enemyMinX)
            {
                boxer.Left = enemyMinX;
                enemySpeed = Math.Abs(enemySpeed);
            }

            if (enemyHealth <= 0)
            {
                TriggerGameOver(GameOverResult.Win);
            }
            if (playerHealth <= 0)
            {
                TriggerGameOver(GameOverResult.Lose);
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                TogglePause();
                return;
            }

            if (e.KeyCode == Keys.R)
            {
                ResetGame();
                return;
            }

            if (isPaused || isGameOver)
            {
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                player.Image = Properties.Resources.boxer_left_punch;
                playerBlock = false;

                if (player.Bounds.IntersectsWith(boxer.Bounds) && enemyBlock == false)
                {
                    DamageEnemy(5);
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                player.Image = Properties.Resources.boxer_right_punch;
                playerBlock = false;

                if (player.Bounds.IntersectsWith(boxer.Bounds) && enemyBlock == false)
                {
                    DamageEnemy(5);
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                player.Image = Properties.Resources.boxer_block;
                playerBlock = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (isPaused || isGameOver)
            {
                return;
            }

            player.Image = Properties.Resources.boxer_stand;
            playerBlock = false;
        }

        private void TogglePause()
        {
            if (isGameOver)
            {
                return;
            }

            if (!isPaused)
            {
                isPaused = true;
                BoxerAttackTimer.Stop();
                BoxerMoveTimer.Stop();
                ShowOverlay("PAUSADO");
            }
            else
            {
                isPaused = false;
                HideOverlay();
                BoxerAttackTimer.Start();
                BoxerMoveTimer.Start();
            }
        }

        private void TriggerGameOver(GameOverResult result)
        {
            if (isGameOver)
            {
                return;
            }

            isGameOver = true;
            pendingResult = result;
            BoxerAttackTimer.Stop();
            BoxerMoveTimer.Stop();

            ShowOverlay(result == GameOverResult.Win ? "VITÓRIA!" : "DERROTA!");
            gameOverDelayTimer.Start();
        }

        private void GameOverDelayTimer_Tick(object? sender, EventArgs e)
        {
            gameOverDelayTimer.Stop();

            if (pendingResult == GameOverResult.Win)
            {
                wins++;
            }
            else
            {
                losses++;
            }

            UpdateScoreLabel();

            string resultText = pendingResult == GameOverResult.Win
                ? "Você venceu!"
                : "Você perdeu!";

            using var gameOver = new GameOverForm(resultText, wins, losses);
            gameOver.ShowDialog(this);

            HideOverlay();

            if (gameOver.SelectedAction == GameOverAction.Restart)
            {
                ResetGame();
            }
            else
            {
                Close();
            }
        }

        private void ResetGame()
        {
            isPaused = false;
            isGameOver = false;
            gameOverDelayTimer.Stop();
            HideOverlay();

            playerHealth = 100;
            enemyHealth = 100;
            playerBlock = false;
            enemyBlock = false;

            player.Image = Properties.Resources.boxer_stand;
            boxer.Image = Properties.Resources.enemy_stand;

            ApplyLayout();
            UpdateHealthBars();

            BoxerAttackTimer.Start();
            BoxerMoveTimer.Start();
        }

        private void UpdateHealthBars()
        {
            playerHealthBar.Value = Math.Max(playerHealthBar.Minimum,
                Math.Min(playerHealthBar.Maximum, playerHealth));

            boxerHealthBar.Value = Math.Max(boxerHealthBar.Minimum,
                Math.Min(boxerHealthBar.Maximum, enemyHealth));
        }

        private void DamagePlayer(int amount)
        {
            playerHealth = Math.Max(0, playerHealth - amount);
        }

        private void DamageEnemy(int amount)
        {
            enemyHealth = Math.Max(0, enemyHealth - amount);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            gameOverDelayTimer?.Stop();
            BoxerAttackTimer?.Stop();
            BoxerMoveTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
}
