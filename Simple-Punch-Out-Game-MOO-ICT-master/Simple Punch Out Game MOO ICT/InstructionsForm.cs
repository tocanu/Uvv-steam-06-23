using System.Drawing;
using System.Windows.Forms;

namespace Simple_Punch_Out_Game_MOO_ICT
{
    public class InstructionsForm : Form
    {
        private readonly Label instructionsLabel;
        private readonly Button closeButton;

        public InstructionsForm()
        {
            Text = "Instruções";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(520, 320);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            instructionsLabel = new Label
            {
                AutoSize = false,
                Width = ClientSize.Width - 40,
                Height = 220,
                Left = 20,
                Top = 20,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Text = "Controles:\n" +
                       "- Seta Esquerda: soco esquerdo\n" +
                       "- Seta Direita: soco direito\n" +
                       "- Seta Baixo: bloquear\n" +
                       "- P: pausar\n" +
                       "- R: reiniciar\n\n" +
                       "Objetivo:\n" +
                       "- Reduzir a vida do inimigo antes que a sua acabe."
            };

            closeButton = new Button
            {
                Text = "Fechar",
                Width = 120,
                Height = 35
            };
            closeButton.Click += (_, __) => Close();

            Controls.Add(instructionsLabel);
            Controls.Add(closeButton);

            Layout += (_, __) => ArrangeLayout();
            ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            closeButton.Left = (ClientSize.Width - closeButton.Width) / 2;
            closeButton.Top = instructionsLabel.Bottom + 10;
        }
    }
}
