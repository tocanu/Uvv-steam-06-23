using System;
using System.Drawing;
using System.Windows.Forms;

namespace Simple_Punch_Out_Game_MOO_ICT
{
    public class MainMenuForm : Form
    {
        private readonly Label titleLabel;
        private readonly Label subtitleLabel;
        private readonly Button startButton;
        private readonly Button instructionsButton;
        private readonly Button exitButton;

        public MainMenuForm()
        {
            Text = "Punch-Out - Menu";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(600, 400);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            titleLabel = new Label
            {
                Text = "Punch-Out",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                AutoSize = true
            };

            subtitleLabel = new Label
            {
                Text = "Jogo 06",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                AutoSize = true
            };

            startButton = new Button
            {
                Text = "Jogar",
                Width = 200,
                Height = 40
            };
            startButton.Click += StartButton_Click;

            instructionsButton = new Button
            {
                Text = "Instruções",
                Width = 200,
                Height = 40
            };
            instructionsButton.Click += InstructionsButton_Click;

            exitButton = new Button
            {
                Text = "Sair",
                Width = 200,
                Height = 40
            };
            exitButton.Click += (_, __) => Close();

            Controls.Add(titleLabel);
            Controls.Add(subtitleLabel);
            Controls.Add(startButton);
            Controls.Add(instructionsButton);
            Controls.Add(exitButton);

            Layout += (_, __) => ArrangeLayout();
            ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            int centerX = ClientSize.Width / 2;

            titleLabel.Left = centerX - titleLabel.Width / 2;
            titleLabel.Top = 40;

            subtitleLabel.Left = centerX - subtitleLabel.Width / 2;
            subtitleLabel.Top = titleLabel.Bottom + 5;

            startButton.Left = centerX - startButton.Width / 2;
            startButton.Top = subtitleLabel.Bottom + 40;

            instructionsButton.Left = centerX - instructionsButton.Width / 2;
            instructionsButton.Top = startButton.Bottom + 15;

            exitButton.Left = centerX - exitButton.Width / 2;
            exitButton.Top = instructionsButton.Bottom + 15;
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            using var game = new Form1();
            Hide();
            game.ShowDialog();
            Show();
        }

        private void InstructionsButton_Click(object? sender, EventArgs e)
        {
            using var instructions = new InstructionsForm();
            instructions.ShowDialog();
        }
    }
}
