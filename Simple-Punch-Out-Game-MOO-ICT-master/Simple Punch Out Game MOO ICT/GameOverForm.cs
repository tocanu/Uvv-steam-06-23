using System.Drawing;
using System.Windows.Forms;

namespace Simple_Punch_Out_Game_MOO_ICT
{
    public enum GameOverAction
    {
        Restart,
        Menu
    }

    public class GameOverForm : Form
    {
        private readonly Label titleLabel;
        private readonly Label scoreLabel;
        private readonly Button restartButton;
        private readonly Button menuButton;

        public GameOverAction SelectedAction { get; private set; } = GameOverAction.Restart;

        public GameOverForm(string resultText, int wins, int losses)
        {
            Text = "Fim de Jogo";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 240);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            titleLabel = new Label
            {
                Text = resultText,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true
            };

            scoreLabel = new Label
            {
                Text = $"Vitórias: {wins}  |  Derrotas: {losses}",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                AutoSize = true
            };

            restartButton = new Button
            {
                Text = "Reiniciar",
                Width = 120,
                Height = 35
            };
            restartButton.Click += (_, __) =>
            {
                SelectedAction = GameOverAction.Restart;
                Close();
            };

            menuButton = new Button
            {
                Text = "Menu",
                Width = 120,
                Height = 35
            };
            menuButton.Click += (_, __) =>
            {
                SelectedAction = GameOverAction.Menu;
                Close();
            };

            Controls.Add(titleLabel);
            Controls.Add(scoreLabel);
            Controls.Add(restartButton);
            Controls.Add(menuButton);

            Layout += (_, __) => ArrangeLayout();
            ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            int centerX = ClientSize.Width / 2;

            titleLabel.Left = centerX - titleLabel.Width / 2;
            titleLabel.Top = 30;

            scoreLabel.Left = centerX - scoreLabel.Width / 2;
            scoreLabel.Top = titleLabel.Bottom + 15;

            restartButton.Left = centerX - restartButton.Width - 10;
            restartButton.Top = scoreLabel.Bottom + 25;

            menuButton.Left = centerX + 10;
            menuButton.Top = scoreLabel.Bottom + 25;
        }
    }
}
