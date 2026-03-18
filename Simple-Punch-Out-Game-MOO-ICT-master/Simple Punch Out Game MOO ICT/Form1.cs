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

        private float currentScale = 1f;
        private int enemyMinX;
        private int enemyMaxX;
        private int enemyTargetX;
        private int enemyModeTicks;
        private EnemyMoveMode enemyMoveMode = EnemyMoveMode.Patrol;
        private int enemyDodgeCooldown;
        private int enemyDodgeBlockTicks;

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

        private enum EnemyMoveMode
        {
            Patrol,
            Pause,
            Dash
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

            EnsureEnemyTargetInBounds();

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

            UpdateEnemyMovement();

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

                if (TryEnemyDodge())
                {
                    return;
                }

                if (player.Bounds.IntersectsWith(boxer.Bounds) && IsEnemyBlocking() == false)
                {
                    DamageEnemy(5);
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                player.Image = Properties.Resources.boxer_right_punch;
                playerBlock = false;

                if (TryEnemyDodge())
                {
                    return;
                }

                if (player.Bounds.IntersectsWith(boxer.Bounds) && IsEnemyBlocking() == false)
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
            enemyDodgeBlockTicks = 0;
            enemyDodgeCooldown = 0;

            player.Image = Properties.Resources.boxer_stand;
            boxer.Image = Properties.Resources.enemy_stand;

            ApplyLayout();
            InitializeEnemyMovement();
            UpdateHealthBars();

            BoxerAttackTimer.Start();
            BoxerMoveTimer.Start();
        }

        private void InitializeEnemyMovement()
        {
            enemyMoveMode = EnemyMoveMode.Patrol;
            enemyModeTicks = 0;
            enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
        }

        private void EnsureEnemyTargetInBounds()
        {
            if (enemyMinX >= enemyMaxX)
            {
                return;
            }

            if (enemyTargetX < enemyMinX || enemyTargetX > enemyMaxX)
            {
                enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
            }
        }

        private void UpdateEnemyMovement()
        {
            if (enemyMinX >= enemyMaxX)
            {
                return;
            }

            if (enemyDodgeCooldown > 0)
            {
                enemyDodgeCooldown--;
            }
            if (enemyDodgeBlockTicks > 0)
            {
                enemyDodgeBlockTicks--;
            }

            if (enemyMoveMode == EnemyMoveMode.Pause)
            {
                enemyModeTicks--;
                if (enemyModeTicks <= 0)
                {
                    enemyMoveMode = EnemyMoveMode.Patrol;
                    enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
                }
                return;
            }

            int baseSpeed = Math.Max(1, (int)(BaseEnemySpeed * currentScale));
            int dashSpeed = baseSpeed + Math.Max(2, (int)(3 * currentScale));
            int speed = enemyMoveMode == EnemyMoveMode.Dash ? dashSpeed : baseSpeed;

            if (enemyMoveMode == EnemyMoveMode.Dash)
            {
                enemyModeTicks--;
                if (enemyModeTicks <= 0)
                {
                    enemyMoveMode = EnemyMoveMode.Patrol;
                    enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
                }
            }

            if (enemyMoveMode == EnemyMoveMode.Patrol && random.NextDouble() < 0.02)
            {
                enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
            }

            int direction = Math.Sign(enemyTargetX - boxer.Left);
            if (direction == 0)
            {
                direction = random.Next(0, 2) == 0 ? -1 : 1;
            }

            enemySpeed = direction * speed;
            boxer.Left += enemySpeed;

            if (boxer.Left >= enemyMaxX)
            {
                boxer.Left = enemyMaxX;
                enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
            }
            else if (boxer.Left <= enemyMinX)
            {
                boxer.Left = enemyMinX;
                enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
            }

            if (Math.Abs(boxer.Left - enemyTargetX) <= speed)
            {
                double roll = random.NextDouble();
                if (roll < 0.25)
                {
                    enemyMoveMode = EnemyMoveMode.Pause;
                    enemyModeTicks = random.Next(8, 18);
                }
                else if (roll < 0.45)
                {
                    enemyMoveMode = EnemyMoveMode.Dash;
                    enemyModeTicks = random.Next(6, 14);
                }
                else
                {
                    enemyMoveMode = EnemyMoveMode.Patrol;
                }

                enemyTargetX = random.Next(enemyMinX, enemyMaxX + 1);
            }
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

        private bool IsEnemyBlocking()
        {
            return enemyBlock || enemyDodgeBlockTicks > 0;
        }

        private bool TryEnemyDodge()
        {
            if (enemyDodgeCooldown > 0 || enemyMoveMode == EnemyMoveMode.Pause)
            {
                return false;
            }

            double chance = enemyMoveMode == EnemyMoveMode.Dash ? 0.7 : 0.5;
            if (random.NextDouble() >= chance)
            {
                return false;
            }

            enemyDodgeCooldown = random.Next(18, 30);
            enemyDodgeBlockTicks = random.Next(8, 16);
            enemyMoveMode = EnemyMoveMode.Dash;
            enemyModeTicks = random.Next(8, 14);

            int playerX = player.Left;
            int mid = (enemyMinX + enemyMaxX) / 2;
            enemyTargetX = playerX < mid ? enemyMaxX : enemyMinX;

            return true;
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
