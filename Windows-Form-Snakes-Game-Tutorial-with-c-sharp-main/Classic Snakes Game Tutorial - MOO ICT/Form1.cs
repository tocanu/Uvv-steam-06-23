using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging; // add this for the JPG compressor

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    public partial class Form1 : Form
    {

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;

        bool isPaused;
        bool wallMode;
        bool progressiveSpeed;

        int baseInterval = 90;
        int minInterval = 30;
        int elapsedSeconds;

        private System.Windows.Forms.Timer clockTimer = null!;

        private Label txtTime = null!;
        private Label txtStatus = null!;
        private Label speedLabel = null!;
        private ComboBox speedCombo = null!;
        private CheckBox wallModeCheck = null!;
        private CheckBox progressiveCheck = null!;

        public Form1()
        {
            InitializeComponent();

            new Settings();

            KeyPreview = true;

            InitializeExtras();
            ApplyLayout();
            Resize += (_, __) => ApplyLayout();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.P)
            {
                if (!startButton.Enabled)
                {
                    TogglePause();
                }
                return;
            }

            if (e.KeyCode == Keys.R)
            {
                RestartGame();
                return;
            }

            if (isPaused)
            {
                return;
            }

            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }



        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (isPaused)
            {
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored: " + score + " and my Highscore is " + highScore + " on the Snake Game from MOO ICT";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.Purple;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game SnapShot MOO ICT";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0,0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }





        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (isPaused)
            {
                return;
            }

            // setting the directions

            if (goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            // end of directions

            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {

                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                    }

                    if (wallMode)
                    {
                        if (Snake[i].X < 0 || Snake[i].X > maxWidth || Snake[i].Y < 0 || Snake[i].Y > maxHeight)
                        {
                            GameOver();
                            return;
                        }
                    }
                    else
                    {
                        if (Snake[i].X < 0)
                        {
                            Snake[i].X = maxWidth;
                        }
                        if (Snake[i].X > maxWidth)
                        {
                            Snake[i].X = 0;
                        }
                        if (Snake[i].Y < 0)
                        {
                            Snake[i].Y = maxHeight;
                        }
                        if (Snake[i].Y > maxHeight)
                        {
                            Snake[i].Y = 0;
                        }
                    }


                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {

                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }

                    }


                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }


            picCanvas.Invalidate();

        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush snakeColour;

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.DarkGreen;
                }

                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
            }


            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
            (
            food.X * Settings.Width,
            food.Y * Settings.Height,
            Settings.Width, Settings.Height
            ));
        }

        private void RestartGame()
        {
            UpdateBoardSize();

            Snake.Clear();

            startButton.Enabled = false;
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;
            elapsedSeconds = 0;
            UpdateTimeLabel();

            isPaused = false;
            txtStatus.Text = "Status: Jogando";
            Settings.directions = "left";
            goLeft = goRight = goUp = goDown = false;

            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head); // adding the head part of the snake to the list

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
            Snake.Add(body);
        }

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight)};

            ApplySpeed();
            gameTimer.Start();
            clockTimer.Start();

        }

        private void EatFood()
        {
            score += 1;

            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

            ApplySpeed();

        }

        private void GameOver()
        {
            gameTimer.Stop();
            clockTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;
            txtStatus.Text = "Status: Game Over";

            if (score > highScore)
            {
                highScore = score;

                txtHighScore.Text = "High Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void InitializeExtras()
        {
            speedLabel = new Label
            {
                AutoSize = true,
                Text = "Velocidade:",
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold)
            };

            speedCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            speedCombo.Items.AddRange(new object[] { "Lento", "Normal", "Rápido" });
            speedCombo.SelectedIndexChanged += (_, __) => UpdateSpeedFromSelection();
            speedCombo.SelectedIndex = 1;

            wallModeCheck = new CheckBox
            {
                AutoSize = true,
                Text = "Modo parede (bater perde)"
            };
            wallModeCheck.CheckedChanged += (_, __) => wallMode = wallModeCheck.Checked;
            wallModeCheck.Checked = false;

            progressiveCheck = new CheckBox
            {
                AutoSize = true,
                Text = "Velocidade progressiva"
            };
            progressiveCheck.CheckedChanged += (_, __) =>
            {
                progressiveSpeed = progressiveCheck.Checked;
                ApplySpeed();
            };
            progressiveCheck.Checked = true;

            txtTime = new Label
            {
                AutoSize = true,
                Text = "Tempo: 00:00",
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold)
            };

            txtStatus = new Label
            {
                AutoSize = true,
                Text = "Status: Pronto",
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold)
            };

            Controls.Add(speedLabel);
            Controls.Add(speedCombo);
            Controls.Add(wallModeCheck);
            Controls.Add(progressiveCheck);
            Controls.Add(txtTime);
            Controls.Add(txtStatus);

            clockTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            clockTimer.Tick += ClockTimer_Tick;

            wallMode = wallModeCheck.Checked;
            progressiveSpeed = progressiveCheck.Checked;
        }

        private void UpdateSpeedFromSelection()
        {
            switch (speedCombo.SelectedIndex)
            {
                case 0:
                    baseInterval = 140;
                    break;
                case 1:
                    baseInterval = 90;
                    break;
                case 2:
                    baseInterval = 60;
                    break;
                default:
                    baseInterval = 90;
                    break;
            }

            ApplySpeed();
        }

        private void ApplySpeed()
        {
            int interval = baseInterval;
            if (progressiveSpeed)
            {
                int bonus = score / 3;
                interval = Math.Max(minInterval, baseInterval - (bonus * 2));
            }

            gameTimer.Interval = interval;
        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            if (isPaused)
            {
                return;
            }

            elapsedSeconds++;
            UpdateTimeLabel();
        }

        private void UpdateTimeLabel()
        {
            int minutes = elapsedSeconds / 60;
            int seconds = elapsedSeconds % 60;
            txtTime.Text = $"Tempo: {minutes:00}:{seconds:00}";
        }

        private void TogglePause()
        {
            if (isPaused)
            {
                isPaused = false;
                txtStatus.Text = "Status: Jogando";
                gameTimer.Start();
                clockTimer.Start();
            }
            else
            {
                isPaused = true;
                txtStatus.Text = "Status: Pausado";
                gameTimer.Stop();
                clockTimer.Stop();
            }
        }

        private void ApplyLayout()
        {
            int margin = 10;
            int minRightPanel = 180;

            int canvasWidth = Math.Max(200, ClientSize.Width - minRightPanel - margin * 2);
            int canvasHeight = Math.Max(200, ClientSize.Height - margin * 2);

            picCanvas.Left = margin;
            picCanvas.Top = margin;
            picCanvas.Width = canvasWidth;
            picCanvas.Height = canvasHeight;

            int panelX = picCanvas.Right + margin;
            int panelWidth = Math.Max(120, ClientSize.Width - panelX - margin);

            startButton.Left = panelX;
            startButton.Top = margin;
            startButton.Width = panelWidth;

            snapButton.Left = panelX;
            snapButton.Top = startButton.Bottom + 10;
            snapButton.Width = panelWidth;

            speedLabel.Left = panelX;
            speedLabel.Top = snapButton.Bottom + 12;

            speedCombo.Left = panelX;
            speedCombo.Top = speedLabel.Bottom + 4;
            speedCombo.Width = panelWidth;

            wallModeCheck.Left = panelX;
            wallModeCheck.Top = speedCombo.Bottom + 10;
            wallModeCheck.Width = panelWidth;

            progressiveCheck.Left = panelX;
            progressiveCheck.Top = wallModeCheck.Bottom + 6;
            progressiveCheck.Width = panelWidth;

            txtScore.Left = panelX;
            txtScore.Top = progressiveCheck.Bottom + 14;

            txtHighScore.Left = panelX;
            txtHighScore.Top = txtScore.Bottom + 10;

            txtTime.Left = panelX;
            txtTime.Top = txtHighScore.Bottom + 10;

            txtStatus.Left = panelX;
            txtStatus.Top = txtTime.Bottom + 10;

            UpdateBoardSize();
            picCanvas.Invalidate();
        }

        private void UpdateBoardSize()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;
        }


    }
}
